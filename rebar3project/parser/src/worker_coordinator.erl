-module(worker_coordinator).
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
    emqttc:subscribe(C, <<"root/initiate">>),
    emqttc:subscribe(C, <<"root/workers">>),
    {ok, {C, []}}.


handle_info({publish, <<"root/initiate">>, Payload}, {C, S}) ->
    Request = binary:split(Payload, <<" ">>),
    io:format("R: ~n~p", [S]),
    Amount = binary_to_integer(hd(tl(Request))),
    io:format("R: ~n~p", [Amount + 1]),
    {NewState, Fufilled} = execute_request(S, [], Amount),
    io:format("Fufilled: ~n~p", [Fufilled]),
    lists:foreach(fun(X) ->
            emqttc:publish(C, hd(Request), X)
        end, Fufilled),
    terminate(normal, normal),
    {noreply, {C, NewState}};

handle_info({publish, <<"root/workers">>, Payload}, {C, S}) ->
    NewState = [Payload] ++ S,
    io:format("NewState: ~n~p", [NewState]),
    {noreply, {C, NewState}};

    
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

%% Returned requested workers
execute_request(S, L, 0) ->
    {S, L};

execute_request([H|T], L, X) ->
    execute_request(T, [H|L], X - 1).













