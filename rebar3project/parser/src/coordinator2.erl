-module(coordinator2).

-behaviour(gen_server).

-define(SERVER, ?MODULE).

%% ------------------------------------------------------------------
%% API Function Exports
%% ------------------------------------------------------------------

-export([start_link/0, stop/0]).

%% ------------------------------------------------------------------
%% gen_server Function Exports
%% ------------------------------------------------------------------

-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
         terminate/2, code_change/3]).

%% ------------------------------------------------------------------
%% API Function Definitions
%% ------------------------------------------------------------------

 start_link() ->
    gen_server:start_link({local, ?SERVER}, ?MODULE, [], []).

stop() ->
    gen_server:call(?SERVER, stop).

%% ------------------------------------------------------------------
%% gen_server Function Definitions
%% ------------------------------------------------------------------

init(_Args) ->
    {ok, C} = emqttc:start_link([{host, "18.216.88.162"},
                                 {client_id, <<"coordinator">>},
                                 {reconnect, 3},
                                 {logger, {console, info}}]),
    %% The pending subscribe
    {ok, C}.

handle_call(stop, _From, State) ->
    {stop, normal, ok, State};

handle_call(_Request, _From, State) ->
    {reply, ok, State}.

handle_cast(_Msg, State) ->
    {noreply, State}.

%% Publish Messages
handle_info(publish, C) ->
    Payload = parser_methods:get_CD(),
    emqttc:publish(C, <<"TopicA">>, Payload, [{qos, 1}]),
    erlang:send_after(3000, self(), publish),
    {noreply, C};

%% Receive Messages
handle_info({publish, Topic, JSON}, C) ->
    %%forward it to parser.
    case jsx:is_json(JSON) of
      true  -> jsx:decode(JSON),
               case parser_methods:get_format(JSON) of
               true ->io:format("Message forward to parser from ~s: ~p~n", [Topic, JSON]);
                                         % get parsed and send to Shaun
               false ->io:format("JSON ERROR: Not DIT029 format\n")
             end;
      false ->io:format("JSON ERROR: Not a valid JSON\n")
    end,
    {noreply, C};

%% Client connected
handle_info({mqttc, C, connected}, C) ->
    io:format("Client ~p is connected~n", [C]),
    emqttc:subscribe(C, <<"TopicA">>, 1),
    self() ! publish,
    {noreply, C};

%% Client disconnected
handle_info({mqttc, C,  disconnected}, C) ->
    io:format("Client ~p is disconnected~n", [C]),
    {noreply, C};

handle_info(_Info, State) ->
    {noreply, State}.

terminate(_Reason, _State) ->
    ok.

code_change(_OldVsn, State, _Extra) ->
    {ok, State}.
