-module(pubsub_coord).
-behaviour(gen_server).
-define(SERVER, ?MODULE).

-export([start/1, stop/0]).
-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
         terminate/2, code_change/3]).

-record(state, {name="shaun",
                stat3,
                reciever,
                message}).

-record(publisher, {client="",
                    topic=""}).

start(SSD) ->
    gen_server:start({local, ?SERVER}, ?MODULE, [SSD], []).


stop() ->
    gen_server:call(?SERVER, stop).
%%% debugging
%test({Name, Node}, Dest, Message) ->
 %   gen_server:call({Name, Node}, {send_message, Dest, Message}, infinity).

init([SSD]) -> 
    {ok, C} = emqttc:start_link([{host, "13.59.108.164"},
                                 {client_id, <<"simpleClient">>},
                                 {logger, info}]),
    start_nodes(SSD),
    P=#publisher{client=C},
    S = SSD,
    {ok, {S, P}}. 

handle_call({send_message, Dest, Message}, _From, S) ->
    NewState = S#state{stat3=prepare_message,reciever=Dest,message=Message},
    publish(NewState),
    send_message(Dest, NewState),
    NewState2 = NewState#state{stat3=idle},
    publish(NewState2),
    {reply, message_sent, NewState2};


handle_call({recieve_message, Message}, _From, S) ->
    timer:sleep(3000),
    NewState = S#state{stat3=recieve_message,message=Message},
    publish(NewState),
    {reply, NewState, recieve_message};


handle_call(terminate, _From, S) ->
    NewState = S#state{stat3=dead},
    publish(NewState),
    {stop, normal, ok, NewState}.

handle_cast({return, _From}, S=#state{}) ->
    {noreply, S#state{}}.
%%input is latin1 :(
handle_info({publish, <<"root/initiate">>, Payload}, {S, P}) ->
    %%RealBin = unicode:characters_to_binary(Payload),
    %%FindBin = string:find(RealBin, "/"),
    %%io:format("new room?: ~n~p", [FindBin]),
    io:format("username: ~n~p", [binary:match(Payload, <<"root/shaun">>, [])]),
    Reg2 = binary:split(Payload, <<"/">>, [global]),
    io:format("new room?: ~n~p", [Reg2]),
    Username = binary:bin_to_list((lists:last(Reg2))),
    io:format("username: ~n~p", [Username]),
    %emqttc:subscribe(P#publisher.client, Payload, 1),
    emqttc:publish(P#publisher.client, Payload, lists:last(Reg2)),
    NewP = P#publisher{topic=Payload},
    {noreply, {S, NewP}};

handle_info({publish, <<"root/initiate">>, Payload}, {S, P}) ->
    io:format("new room?: ~n~p", [Payload]),
    emqttc:subscribe(P#publisher.client, Payload, 1),
    NewP = P#publisher{topic=Payload},
    %emqttc:publish(P#publisher.client, Payload, list_to_binary(S#state.name)),

    {noreply, {S, NewP}};

handle_info({publish, Topic, Payload}, {S, P}) when Topic =:= P#publisher.topic ->
    emqttc:unsubscribe(P#publisher.client, Topic),
    emqttc:publish(P#publisher.client, <<"root/initiate">>, <<"next">>),

    {noreply, {S, P}};

handle_info({publish, Topic, <<"Taken">>}, {S, P}) when Topic =:= P#publisher.topic ->
    io:format("this room is taken~n~p", [P#publisher.topic]),
    emqttc:unsubscribe(P#publisher.client, Topic),
    emqttc:publish(P#publisher.client, <<"root/initiate">>, <<"next">>),

    {noreply, {S, P}};

handle_info({publish, Topic, Payload}, {S, P}) when Topic =:= P#publisher.topic ->
    NewS = S#state{name=Payload, stat3="idle ready"},
    %L = maps:to_list(S),
    emqttc:publish(P#publisher.client, Topic, <<"Taken">>),
    {noreply, {NewS, P}};
%handle_info({publish, Topic, "Taken"}, {S, P}) ->
 %   emqttc:unsubscribe(P#publisher.client, Topic),
  %  {noreply, S#state{}};

handle_info(Msg, S) ->
    io:format("what is this?! ~p~n",[Msg]),
    {noreply, S}.

terminate(normal, _S=#state{}) ->
    ok.

code_change(_OldVsn, State, _Extra) ->
    {ok, State}. 

%%% Private functions
send_message(DestNode, S) ->
    timer:sleep(3000),
    gen_server:call(DestNode, {recieve_message, S#state.message}),
    NewState = S#state{stat3=send_message},
    publish(NewState),
    timer:sleep(2800).

publish(S) ->
    %Payload to binary
    %emqttc:publish(#publisher.client, #publisher.topic, term_to_binary(Payload), qos2).
    io:format("state: ~n~p", [S#state{}]).

start_nodes(Name) ->
    this.
