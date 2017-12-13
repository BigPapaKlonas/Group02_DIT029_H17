-module(diagram_intiator).

-behaviour(gen_server).

-define(SERVER, ?MODULE).

%% ------------------------------------------------------------------
%% API Function Exports
%% ------------------------------------------------------------------

-export([start_link/0, start/0, stop/0]).

%% ------------------------------------------------------------------
%% gen_server Function Exports
%% ------------------------------------------------------------------

-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
         terminate/2, code_change/3]).

%% ------------------------------------------------------------------
%% API Function Definitions
%% ------------------------------------------------------------------
start() ->
   gen_server:start_link({local, ?SERVER}, ?MODULE, [], []).

 start_link() ->
    gen_server:start_link({local, ?SERVER}, ?MODULE, [], []).

stop() ->
    gen_server:call(?SERVER, stop).

%% ------------------------------------------------------------------
%% gen_server Function Definitions
%% ------------------------------------------------------------------

init(_Args) ->
    {ok, C} = emqttc:start_link([{host, "13.59.108.164"},
                                 {client_id, float_to_binary(rand:normal())},
                                 {reconnect, 3},
                                 {logger, {console, info}},
                                 {keepalive, 0}]),
    {ok, {C, []}}.

handle_call(stop, _From, State) ->
    {stop, normal, ok, State};

handle_call(_Request, _From, State) ->
    {reply, ok, State}.

handle_cast(_Msg, State) ->
    {noreply, State}.

%% Publish Messages
%handle_info(publish, C) ->
%    Payload = parser:get_CD(),
%    emqttc:publish(C, <<"TopicA">>, Payload, [{qos, 1}]),
%    erlang:send_after(3000, self(), publish),
%    {noreply, C};

%% Receive Messages
handle_info({publish, <<"root/newdiagram">>, Room}, {C, S}) ->
    io:format("~n Room ~n~p", [Room]),
    emqttc:publish(C, hd(S), Room),
    {noreply, {C, tl(S)}};

%% Client connected
handle_info({mqttc, C, connected}, {C, S}) ->
    io:format("Client ~p is connected~n", [C]),
    emqttc:subscribe(C, <<"root/newdiagram">>),
    emqttc:subscribe(C, <<"root/newdiagram/workers">>),
    {noreply, {C, S}};

%% Client disconnected
handle_info({mqttc, C,  disconnected}, {C, S}) ->
    io:format("Client ~p is disconnected~n", [C]),
    {noreply, {C, S}};

handle_info({publish, <<"root/newdiagram/workers">>, Payload}, {C, S}) ->
    NewState = [Payload] ++ S,
    io:format("NewState: ~n~p", [NewState]),
    {noreply, {C, NewState}};

handle_info(_Info, State) ->
    {noreply, State}.

terminate(_Reason, _State) ->
    ok.

code_change(_OldVsn, State, _Extra) ->
    {ok, State}.
