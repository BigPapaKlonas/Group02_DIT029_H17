-module(supervisor_parent).
-behaviour(supervisor).

%% API
-export([start_link/0]).

%% Supervisor callbacks
-export([init/1]).

-define(SUP, ?MODULE).

%% API functions
start_link() -> 
	supervisor:start_link({local, ?SUP}, ?MODULE, []).

%% Supervisor callbacks
%% Child :: {Id, StartFunc, Restart, Shutdown, Type, Modules}
init([]) ->
	 % Max 5 restart within 50ms before supervisor gives up
	 {ok, { {one_for_al, 5, 60}, 
		   [#{id => sim,							% Unique Identifier for child
			 start => {simulation_supervisor, start_link, []},
			 restart => permanent,					% Permanent process should always be restarted
             shutdown => infinity,					% Set to infinity to give time for sub-sup to shutdown
             type => supervisor,					% Specify wheter child is worker or supervisor
             modules => [simulation_supervisor]		% Specifing the child's callback module
			}
		  ]}
	 }.
