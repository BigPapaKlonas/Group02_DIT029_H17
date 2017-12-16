-module(tcp_supervisor).
-behaviour(supervisor).

-export([start_link/0, start_socket/0]).
-export([init/1]).

start_link() ->
  supervisor:start_link({local, ?MODULE}, ?MODULE, []).

init([]) ->
  {ok, ListenSocket} = gen_tcp:listen(3000, [{active,once}]),
  %% We start our pool of empty listeners.
  %% We must do this in another, as it is a blocking process.
  spawn_link(fun empty_listeners/0),
  {ok, { {simple_one_for_one, 60, 3600},
         [
          {tcp_server, {tcp_server, start_link, [ListenSocket]}, temporary, 1000, worker, [tcp_server]}
         ]
       } }.

start_socket() ->
  supervisor:start_child(?MODULE, []).

%% Start with 20 listeners so that many multiple connections can
%% be started at once, without serialization. In best circumstances,
%% a process would keep the count active at all times to insure nothing
%% bad happens over time when processes get killed too much.
empty_listeners() ->
  [start_socket() || _ <- lists:seq(1,20)],
  ok.
