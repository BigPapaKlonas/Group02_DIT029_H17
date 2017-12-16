%%%-------------------------------------------------------------------
%% @doc worker_coordinator top level supervisor.
%% @end
%%%-------------------------------------------------------------------

-module(worker_coordinator_sup).

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
    {ok, {{one_for_all, 1, 60}, 
    	[{user,
		{worker_coordinator, start, []},
		permanent, 1000, worker, [worker_coordinator]}
		]}}.

%%====================================================================
%% Internal functions
%%====================================================================
