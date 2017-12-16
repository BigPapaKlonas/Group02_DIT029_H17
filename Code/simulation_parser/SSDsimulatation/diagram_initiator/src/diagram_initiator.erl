-module(diagram_initiator).
-behaviour(gen_server).

-define(SERVER, ?MODULE).

-export([start/0, stop/0]).

-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
terminate/2, code_change/3]).

-record(state, {c,
                topic, 
                room, 
                nodelist, 
                nodemap,
                ssd}).

start() ->
    gen_server:start({local, ?SERVER}, ?MODULE, [], []).

stop() ->
    gen_server:call(?SERVER, stop).

init([]) ->
    {ok, C} = emqttc:start_link([{host, "13.59.108.164"},
                                 {client_id, float_to_binary(rand:normal())},
                                 {logger, info}]),
    emqttc:subscribe(C, <<"root">>),
    State=#state{c=C},
    {ok, State}.

handle_info({publish, Topic, <<"next">>}, S) when length(S#state.ssd) =:= 0->
    timer:sleep(4000),
    io:format("CurrentMessage: ~n~p", [S#state.ssd]),
    timer:sleep(4000),
    L = maps:values(S#state.nodemap),
    lists:foreach(fun(X) ->
            emqttc:publish(S#state.c, X, <<"finished">>)
        end, L),
    stop(),
    {noreply, S};

handle_info({publish, Topic, <<"next">>}, S) ->
    timer:sleep(4000),
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

handle_info({publish, <<"root">>, Payload}, S) ->
    WholeSSD = parser:get_parsed_diagram(Payload),
    NodeList = hd(tl(WholeSSD)),
    SSD = hd(hd(lists:reverse(tl(lists:reverse(WholeSSD))))),
    io:format("SSD: ~n~p", [SSD]),
    Room = <<"root/shaun/diagram">>,
    CoordRoom = <<Room/binary, <<"/">>/binary, <<"coordinator">>/binary>>,
    Nodes = maps:new(),
    State=S#state{topic=CoordRoom,room=Room,nodelist=NodeList,nodemap=Nodes,ssd=SSD},
    emqttc:subscribe(State#state.c, CoordRoom),
    Amount = integer_to_binary(length(State#state.nodelist)),
    Request = <<CoordRoom/binary, <<" ">>/binary, Amount/binary>>,
    io:format("Request: ~n~p", [Request]),
    io:format("CoordRoom: ~n~p", [State#state.topic]),
    emqttc:publish(State#state.c, <<"root/initiate">>, Request),
    {noreply, State};

    
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
    BinName = binary:list_to_bin(Name ++ lists:flatten(NodeNumber)),
    Room = S#state.room,
    UserRoom = <<Room/binary, <<"/">>/binary, BinName/binary>>,
    emqttc:publish(S#state.c, UserRoom, <<"initial">>, [{qos, 1}, {retain, true}]),
    emqttc:publish(S#state.c, Worker, UserRoom, [{qos, 1}, {retain, true}]),
    NewState = S#state{nodelist=tl(S#state.nodelist),nodemap=maps:put(Name, UserRoom, S#state.nodemap)},
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
    BinName = binary:list_to_bin(Name ++ lists:flatten(NodeNumber)),
    Room = S#state.room,
    UserRoom = <<Room/binary, <<"/">>/binary, BinName/binary>>,
    io:format("map: ~n~p", [UserRoom]),
    emqttc:publish(S#state.c, UserRoom, <<"initial">>, [{qos, 1}, {retain, true}]),
    emqttc:publish(S#state.c, Worker, UserRoom,[{qos, 1}, {retain, true}]),
    NewState = S#state{nodelist=tl(S#state.nodelist),nodemap=maps:put(Name, UserRoom, S#state.nodemap)},
    NewState.

%%Coordinator











