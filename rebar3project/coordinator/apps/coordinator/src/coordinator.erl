-module(coordinator).

%% ====================================================================
%% API functions
%% ===================================================================
-export([start/0, publish/2, subscribe/1]).

start() ->


	%% connect to broker
	{ok, C} = emqttc:start_link([{host, "localhost"}, {client_id, <<"simpleClient">>}]),


	%% subscribe
	emqttc:subscribe(C, <<"TopicA">>, qos2),

	%% publish
	emqttc:publish(C, <<"TopicA">>, <<"Shuannnnnn">>),

	%% receive message
	receive
	    {publish, Topic, Payload} ->
	        io:format("Message Received from ~s: ~p~n", [Topic, Payload])
	after
	    1000 ->
	        io:format("Error: receive timeout!~n")
	end,

	%% disconnect from broker
	emqttc:disconnect(C).

publish(T, M) ->
		{ok, C} = emqttc:start_link([{host, "localhost"},
															 {client_id, <<"simpleClient">>},
															 {reconnect, 3},
															 {logger, {console, info}}]),
		emqttc:publish(C, T, M).

subscribe(T) ->

		{ok, C} = emqttc:start_link([{host, "localhost"}, {client_id, <<"simpleClient">>}]),
		emqttc:subscribe(C, T, qos2),
		receive
		    {publish, Topic, Payload} ->
		        io:format("Message Received from ~s: ~p~n", [Topic, Payload])
		after
		    1000 ->
		        io:format("Error: receive timeout!~n")
		end,

		%% disconnect from broker
		emqttc:disconnect(C).
