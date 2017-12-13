-module(diagram_executer).
-behaviour(gen_server).

-define(SERVER, ?MODULE).

-export([start/1, stop/0]).

-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
terminate/2, code_change/3]).

-record(state, {c,
                topic, 
                room, 
                nodelist, 
                nodemap,
                ssd,
                mainroom,
                wholessd}).


start(Room) ->
    gen_server:start_link({local, ?SERVER}, ?MODULE, [Room], []).

stop() ->
    gen_server:call(?SERVER, stop).

init([Room]) ->
    {ok, C} = emqttc:start_link([{host, "13.59.108.164"},
                                 {client_id, float_to_binary(rand:normal())},
                                 {logger, info}]),
    emqttc:subscribe(C, Room),
    State=#state{c=C, mainroom=Room},
    {ok, State}.

handle_info({publish, Topic, <<"next">>}, S) when length(S#state.ssd) =:= 0->
    timer:sleep(5000),
    io:format("CurrentMessage: ~n~p", [S#state.ssd]),
    timer:sleep(4000),
    L = maps:values(S#state.nodemap),
    lists:foreach(fun(X) ->
            emqttc:publish(S#state.c, X, <<"finished">>)
        end, L),
    emqttc:publish(S#state.c, S#state.mainroom, <<"finished">>),
    stop(),
    {noreply, S};

handle_info({publish, Topic, <<"next">>}, S) ->
    timer:sleep(5000),
    io:format("CurrentMessage: ~n~p", [S#state.ssd]),
    CurrentMessage = hd(S#state.ssd),
    {_SFrom, Fro} = hd(CurrentMessage),
    From = maps:get(Fro, S#state.nodemap),
    {_SMessage, Messag} = hd(tl(CurrentMessage)),
    Message = list_to_binary(Messag),
    {_STo, NameTo} = lists:last(CurrentMessage), 
    To = maps:get(NameTo, S#state.nodemap),
    Room = S#state.room,
    FromLocation = From,
    CompleteMessage = <<Message/binary, <<"@">>/binary, To/binary>>,
    emqttc:publish(S#state.c, FromLocation, CompleteMessage),
    NewS = S#state{ssd = lists:delete(hd(S#state.ssd), S#state.ssd)},
    {noreply, NewS};


handle_info({publish, Topic, Payload}, S) when Topic =:= S#state.topic ->
    NewS = create_node(S, Payload),
    {noreply, NewS};

handle_info({publish, Topic, Payload}, S) when Topic =:= S#state.mainroom ->
    WholeSSD = parser:get_parsed_diagram(Payload),
    Proc = parser:get_processes(parser:decode_map(Payload)),
    NodeList = hd(tl(WholeSSD)),
    NodeListBinary = term_to_binary(Proc),
    %emqttc:publish(S#state.c, <<"root/processes">>, NodeListBinary),
    emqttc:unsubscribe(S#state.c, S#state.mainroom),
    ParSSD = tl(hd(lists:reverse(tl(lists:reverse(WholeSSD))))),
    LengthPar = integer_to_binary(length(ParSSD)),
    if length(ParSSD) == 0 ->
        Par = <<"nopar">>;
        true ->
            Par = <<"par">>
        end,
    SSD = hd(hd(lists:reverse(tl(lists:reverse(WholeSSD))))),
    LengthWhole = integer_to_binary(length(ParSSD) + length(SSD)),
    io:format("SSD: ~n~p", [ParSSD]),
    Room = S#state.mainroom,
    CoordRoom = <<Room/binary, <<"/">>/binary, <<"coordinator">>/binary>>,
    Nodes = maps:new(),
    State=S#state{topic=CoordRoom,room=Room,nodelist=NodeList,nodemap=Nodes,ssd=SSD},
    Amount = integer_to_binary(length(NodeList)),
    SpawnMsg = << Par/binary, <<" ">>/binary, Amount/binary, <<" ">>/binary, LengthPar/binary, <<" ">>/binary, LengthWhole/binary>>,
    State2=State#state{wholessd=SpawnMsg},
    emqttc:subscribe(State#state.c, CoordRoom),
    Request = <<CoordRoom/binary, <<" ">>/binary, Amount/binary>>,
    io:format("Request: ~n~p", [Request]),
    io:format("~nCoordRoom: ~n~p", [State#state.topic]),
    emqttc:publish(State#state.c, <<"root/initiate">>, Request),
    timer:sleep(5000),
    emqttc:publish(State2#state.c, Room, State2#state.wholessd),
    {noreply, State2};

    
handle_info(Info, State) ->
    io:format("ignored: ~p~n", [Info]),
    {noreply, State}.

handle_call(terminate, _From, Nodes) ->
    {stop, normal, ok, Nodes}.

handle_cast({something, _From}, Nodes) ->
    {noreply, Nodes}.

terminate(_Reason, _State) ->
    ok.

code_change(_OldVsn, State, _Extra) ->
    {ok, State}.


create_node(S, Worker) when length(S#state.nodelist) =:= 1 ->
    Number = abs(rand:normal()),
    NodeNumber = io_lib:format("~p",[Number]),
    {_Title, Name} = hd(tl(hd(S#state.nodelist))),
    ActName = binary:list_to_bin(Name),
    Uid = binary:list_to_bin(lists:flatten(NodeNumber)),
    BinName = <<ActName/binary, <<":">>/binary, Uid/binary>>,
    Room = S#state.room,
    UserRoom = <<Room/binary, <<"/">>/binary, BinName/binary>>,
    emqttc:publish(S#state.c, UserRoom, <<"initial">>, [{qos, 1}, {retain, true}]),
    emqttc:publish(S#state.c, Worker, UserRoom, [{qos, 1}, {retain, true}]),
    NewState = S#state{nodelist=tl(S#state.nodelist),nodemap=maps:put(Name, UserRoom, S#state.nodemap)},
    NodeListBin = return_node_bin_list(maps:values(NewState#state.nodemap)),
    emqttc:publish(S#state.c, Room, NodeListBin),
    emqttc:publish(S#state.c, NewState#state.topic, <<"next">>),
    emqttc:unsubscribe(S#state.c, <<"root/initiate">>),
    io:format("map: ~n~p", [NewState#state.nodemap]),
    NewState;

create_node(S, Worker) ->
    R = rand:uniform(3000),
    timer:sleep(R),
    Number = abs(rand:normal()),
    NodeNumber = io_lib:format("~p",[Number]),
    io:format("map: ~n~p", [hd(tl(hd(S#state.nodelist)))]),
    {_Title, Name} = hd(tl(hd(S#state.nodelist))),
    ActName = binary:list_to_bin(Name),
    Uid = binary:list_to_bin(lists:flatten(NodeNumber)),
    BinName = <<ActName/binary, <<":">>/binary, Uid/binary>>,
    Room = S#state.room,
    UserRoom = <<Room/binary, <<"/">>/binary, BinName/binary>>,
    io:format("map: ~n~p", [UserRoom]),
    emqttc:publish(S#state.c, UserRoom, <<"initial">>, [{qos, 1}, {retain, true}]),
    emqttc:publish(S#state.c, Worker, UserRoom,[{qos, 1}, {retain, true}]),
    NewState = S#state{nodelist=tl(S#state.nodelist),nodemap=maps:put(Name, UserRoom, S#state.nodemap)},
    NewState.

%%Coordinator

return_node_bin_list([H|T]) ->
    Name = lists:last(binary:split(H, <<"/">>, [global])),
    return_node_bin_list(T, <<Name/binary>>).

return_node_bin_list([], B) ->
    B;

return_node_bin_list([H|T], B) ->
    Name = lists:last(binary:split(H, <<"/">>, [global])),
    return_node_bin_list(T ,<<B/binary, <<" ">>/binary, Name/binary>>).











