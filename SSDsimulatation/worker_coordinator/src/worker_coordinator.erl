-module(worker_coordinator).
-behaviour(gen_server).

-define(SERVER, ?MODULE).

-export([start/0, stop/0]).

-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
terminate/2, code_change/3]).


start() ->
    gen_server:start({local, ?SERVER}, ?MODULE, [], []).

stop() ->
    gen_server:call(?SERVER, stop).

init([]) ->
    {ok, C} = emqttc:start_link([{host, "13.59.108.164"},
                                 {client_id, float_to_binary(rand:normal())},
                                 {logger, info}]),
    emqttc:publish(C, <<"root/processes">>, <<"10">>),
    {ok, {C, []}}.


handle_info({publish, <<"root/initiate">>, Payload}, {C, S}) when S < 10 ->
    emqttc:publish(C, <<"root/processes">>, <<"10">>),
    emqttc:publish(C, <<"root/initiate">>, Payload),
    {noreply, {C, S}};

handle_info({publish, <<"root/initiate">>, Payload}, {C, S}) ->
    Request = binary:split(Payload, <<" ">>),
    io:format("R: ~n~p", [Request]),
    Amount = binary_to_integer(hd(tl(Request))),
    {NewState, Fufilled} = execute_request(S, [], Amount),
    io:format("Fufilled: ~n~p", [Fufilled]),
    lists:foreach(fun(X) ->
            emqttc:publish(C, hd(Request), X)
        end, Fufilled),
    {noreply, {C, NewState}};

handle_info({publish, <<"root/workers">>, <<"clean">>}, {C, S}) ->
    lists:foreach(fun(X) ->
            emqttc:publish(C, X, <<"status">>)
        end, S),
    {noreply, {C, []}};

handle_info({publish, <<"root/workers">>, Payload}, {C, S}) ->
    NewState = [Payload] ++ S,
    io:format("New Worker: ~n~p", [NewState]),
    {noreply, {C, NewState}};


%% Client connected
handle_info({mqttc, C, connected}, {C, S}) ->
    io:format("Client ~p is connected~n", [C]),
    emqttc:subscribe(C, <<"root/initiate">>),
    emqttc:subscribe(C, <<"root/workers">>),
    {noreply, {C, S}};

%% Client disconnected
handle_info({mqttc, C,  disconnected}, {C, S}) ->
    io:format("Client ~p is disconnected~n", [C]),
    {noreply, {C, S}};

    
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













