-module(pubsub_ssdchat).
-behaviour(gen_server).
-define(SERVER, ?MODULE).

-export([start/0, start_link/0, stop/0]).
-export([init/1, handle_call/3, handle_cast/2, handle_info/2,
         terminate/2, code_change/3, set_name_topic/1]).

-record(state, {name,
                stat3,
                reciever=
                 <<"">>,
                message= <<"">>}).

-record(publisher, {client,
                    topic,
                    origin}).

start() ->
    %publish_node(),
    gen_server:start({local, ?SERVER}, ?MODULE, [], []).

start_link() ->
    gen_server:start_link({local, ?SERVER}, ?MODULE, [], []).

stop() ->
	%%pubsub_ssdchat_app:stop(stop).
    gen_server:stop(?SERVER).
%%% debugging
%test({Name, Node}, Dest, Message) ->
 %   gen_server:call({Name, Node}, {send_message, Dest, Message}, infinity).

init([]) -> 
	UniqueName = float_to_binary(rand:normal()),
    {ok, C} = emqttc:start_link([{host, "13.59.108.164"},
                                 {client_id, float_to_binary(rand:normal())},
                                 {logger, info}]),
    OrignalRoom = << <<"root/workers/">>/binary, UniqueName/binary>>,
    emqttc:subscribe(C, OrignalRoom),
    emqttc:publish(C, <<"root/workers">>, OrignalRoom),
    P=#publisher{client=C, origin=OrignalRoom},
    S=#state{},
    {ok, {S, P}}. 

handle_call({send_message, Dest, Message}, _From, S) ->
    NewState = S#state{stat3=prepare_message,reciever=Dest,message=Message},
    send_message(Dest, NewState),
    NewState2 = NewState#state{},
    {reply, message_sent, NewState2};


handle_call({recieve_message, Message}, _From, S) ->
    timer:sleep(3000),
    NewState = S#state{},
    {reply, NewState, recieve_message};


handle_call(terminate, _From, S) ->
    NewState = S#state{},
    {stop, normal, ok, NewState}.

handle_cast({return, _From}, S=#state{}) ->
    {noreply, S#state{}}.

handle_info({publish, <<"root/initiate">>, Payload}, {S, P}) ->
    io:format("new room?: ~n~p", [Payload]),
    %emqttc:subscribe(P#publisher.client, Payload),
    NewP = P#publisher{topic=Payload},
    emqttc:unsubscribe(P#publisher.client, <<"root/initiate">>),
    R = rand:uniform(30000),
    timer:sleep(R),
    emqttc:subscribe(P#publisher.client, Payload),

    {noreply, {S, NewP}};

%handle_info({publish, Topic, Payload}, {S, P}) when Topic =:= P#publisher.topic ->
 %   emqttc:unsubscribe(P#publisher.client, Topic),
  %  emqttc:publish(P#publisher.client, <<"root/initiate">>, <<"next">>),

   % {noreply, {S, P}};
handle_info({publish, Topic, <<"status">>}, {S, P}) when P#publisher.origin =:= Topic ->
    timer:sleep(2000),
    emqttc:publish(P#publisher.client, <<"root/workers">>, Topic),
    {noreply, {S, P}};

handle_info({publish, Topic, Payload}, {S, P}) when P#publisher.origin =:= Topic ->
    NewP = P#publisher{topic=Payload, origin="finished"},
    emqttc:subscribe(NewP#publisher.client, Payload),
    {noreply, {S, NewP}};

handle_info({publish, Topic, <<"initial">>}, {S, P}) ->
	Name = lists:last(binary:split(Topic, <<"/">>, [global])),
	Stat3 = <<"initial">>,
	PubRoom = set_name_topic(Topic),
	NewP = P#publisher{topic=PubRoom},
    NewS = S#state{name=Name,stat3=Stat3,reciever = <<"">>,message = <<"">>},
    publish_state(NewS, NewP),
    io:format("this room is mine~n~p", [P#publisher.topic]),
    %L = maps:to_list(S),
    Stat32 = <<"idle">>,
    NewSS = S#state{name=Name,stat3=Stat32},
    {noreply, {NewSS, NewP}};
%handle_info({publish, Topic, "Taken"}, {S, P}) ->
 %   emqttc:unsubscribe(P#publisher.client, Topic),
  %  {noreply, S#state{}};

handle_info({publish, Topic, <<"finished">>}, {S, P}) ->
	UniqueName = float_to_binary(rand:normal()),
    OrignalRoom = << <<"root/workers/">>/binary, UniqueName/binary>>,
    emqttc:subscribe(P#publisher.client, OrignalRoom),
    emqttc:publish(P#publisher.client, <<"root/workers">>, OrignalRoom),
    NewP=P#publisher{origin=OrignalRoom},
    {noreply, {S, NewP}};

handle_info({publish, Topic, <<"idle">>}, {S, P}) ->
	NewS = S#state{stat3= <<"idle">>},
	publish_state(NewS, P),
	stop(),
    {noreply, {NewS, P}};

handle_info({publish, Topic, Payload}, {S, P}) when Payload /= <<"taken">> ->
	MsgCheck = binary:match(Payload, <<"@">>),
	if nomatch == MsgCheck ->
		timer:sleep(1500),
		io:format("~n recieved: ~n~p", [Payload]),
		BuggyPOS = float_to_binary(rand:normal()),
		Room = P#publisher.topic,
		Coord = <<Room/binary, <<"/coordinator">>/binary>>,
		NewS = S#state{stat3 = <<"recievedmessage">>,reciever = BuggyPOS,message = <<"">>},
		publish_state(NewS, P),
		emqttc:publish(P#publisher.client, Coord, <<"next">>);
		true -> 
			MessageSplit = binary:split(Payload, <<"@">>),
			Reciever = lists:last(binary:split(hd(tl(MessageSplit)), <<"/">>, [global])),
			Prep = S#state{stat3= <<"preparemessage">>, reciever=Reciever,message=hd(MessageSplit)},
			io:format("~n sending: ~n~p", [Payload]),
			publish_state(Prep, P),
			timer:sleep(1000),
			NewS = Prep#state{stat3= <<"sentmessage">>},
			publish_state(NewS, P),
			emqttc:publish(P#publisher.client, hd(tl(MessageSplit)), hd(MessageSplit))
		end,
    {noreply, {NewS, P}};

%% Client connected
handle_info({mqttc, C, connected}, {S, P}) ->
    io:format("Client ~p is connected~n", [C]),
    emqttc:subscribe(C, P#publisher.origin),
    {noreply, {S, P}};

%% Client disconnected
handle_info({mqttc, C,  disconnected}, {S, P}) ->
    io:format("Client ~p is disconnected~n", [C]),
    {noreply, {S, P}};

handle_info(Msg, S) ->
    io:format("what is this?! ~p~n",[Msg]),
    {noreply, S}.

terminate(normal, _S=#state{}) ->
    ok.

code_change(_OldVsn, State, _Extra) ->
    {ok, State}. 

%%% Private functions
set_name_topic(Room) ->
	L = binary:split(Room, <<"/">>, [global]),
	LNew = lists:reverse(tl(lists:reverse(L))),
	%io:format("pre: ~n~p", [LNew]),
	concat_bin(tl(LNew), hd(LNew)).
	%io:format("post: ~n~p", [Fin]).

concat_bin([], Acc) ->
	Acc;
concat_bin([H|T], Acc) ->
	concat_bin(T, <<Acc/binary, <<"/">>/binary, H/binary>>).


send_message(DestNode, S) ->
    timer:sleep(3000),
    gen_server:call(DestNode, {recieve_message, S#state.message}),
    NewState = S#state{stat3=send_message},
    timer:sleep(2800).

publish_state(S, P) ->
	R = rand:uniform(3000),
    timer:sleep(R),
	io:format("state: ~n~p", [S]),
	Name = S#state.name,
	Stat3 = S#state.stat3,
	Reciever = S#state.reciever,
	io:format("state: ~n~p", [Reciever]),
	Message = S#state.message,
	io:format("state: ~n~p", [Message]),
	PartState = <<Name/binary, <<" ">>/binary, Stat3/binary, <<" ">>/binary>>,
	State = <<PartState/binary, Reciever/binary, <<" ">>/binary, Message/binary>>,
    %Payload to binary
    emqttc:publish(P#publisher.client, P#publisher.topic, State, qos2),
    io:format("state: ~n~p", [State]).

publish_node(Name, C) ->
    io:format("state: ~n~p", [Name]),
    %T = term_to_binary(Name),
    R = io_lib:format("~p",[Name]),
    lists:flatten(R),
    io:format("string: ~n~p", [R]),
    T = hd(R),
    io:format("string head: ~n~p", [T]),
    %S = string:split(T, "@"),
    %S = string:split(R, "@"),
    %L = [B1,Name],
    emqttc:publish(C, <<"root/noderizor">>, term_to_binary(Name)).
