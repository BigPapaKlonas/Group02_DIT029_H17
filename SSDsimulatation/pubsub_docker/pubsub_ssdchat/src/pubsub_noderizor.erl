-module(pubsub_noderizor).
-behaviour(gen_server).

-define(SERVER, ?MODULE).

-export([start/2, stop/0]).

-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
terminate/2, code_change/3]).

-record(state, {c, 
                room, 
                nodelist, 
                nodemap,
                ssd}).

start(SSD, Room) ->
    gen_server:start({local, ?SERVER}, ?MODULE, [SSD, Room], []).

stop() ->
    gen_server:call(?SERVER, stop).

init([SSD, Room]) ->
    {ok, C} = emqttc:start_link([{host, "18.216.88.162"},
                                 {client_id, float_to_binary(rand:normal())},
                                 {logger, info}]),
    emqttc:subscribe(C, <<"root/initiate">>),
    NodeList = hd(tl(SSD)),
    Nodes = maps:new(),
    State=#state{c=C,room=Room,nodelist=NodeList,nodemap=Nodes,ssd=SSD},
    NextState = create_node(State),
    {ok, NextState}.

handle_info({publish, <<"root/initiate">>, <<"next">>}, S) ->
    NewS = create_node(S),
    {noreply, NewS};
    
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


create_node(S) when length(S#state.nodelist) =:= 1 ->
    NodeNumber = abs(rand:normal()),
    {name, Name} = hd(tl(hd(S#state.nodelist))),
    UserRoom = S#state.room ++ Name,
    emqttc:publish(S#state.c, <<"root/initiate">>, binary:list_to_bin(UserRoom)),
    NewState = S#state{nodelist=tl(S#state.nodelist),nodemap=maps:put(Name, NodeNumber)},
    emqttc:publish(S#state.c, <<"root/initiate">>, <<"finished">>),
    io:format("map: ~n~p", [NewState#state.nodemap]);

create_node(S) ->
    NodeNumber = abs(rand:normal()),
    {name, Name} = hd(tl(hd(S#state.nodelist))),
    UserRoom = S#state.room ++ Name,
    emqttc:publish(S#state.c, <<"root/initiate">>, binary:list_to_bin(UserRoom)),
    NewState =#state{nodelist=tl(S#state.nodelist),nodemap=maps:put(Name, NodeNumber)},
    NewState.









