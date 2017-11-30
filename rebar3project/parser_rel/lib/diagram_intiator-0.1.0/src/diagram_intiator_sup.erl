%%%-------------------------------------------------------------------
%% @doc diagram_intiator top level supervisor.
%% @end
%%%-------------------------------------------------------------------

-module(diagram_intiator_sup).

-behaviour(supervisor).

%% API
-export([start_link/0]).

%% Supervisor callbacks
-export([init/1]).

-define(SERVER, ?MODULE).

%%====================================================================
%% API functions
%%====================================================================

start_link() ->
    supervisor:start_link({local, ?SERVER}, ?MODULE, []).

%%====================================================================
%% Supervisor callbacks
%%====================================================================

%% Child :: {Id,StartFunc,Restart,Shutdown,Type,Modules}
init([]) ->
  {ok, { {one_for_all, 3, 60},
      [#{id => diagram_intiator,
          start => {diagram_intiator, start, []},
          restart => permanent,
          shutdown => 1000,
          type => worker,
          modules => [diagram_intiator]}]} }.

%%====================================================================
%% Internal functions
%%====================================================================
