%% @author stirb
%% @doc Supervises the Simulation Cloud i.e Parser, Diagram simulator and MQTTClientCoordinator.

-module(simulation_supervisor).
-behaviour(supervisor).

%% API
-export([start_link/0]).

%% Supervisor callbacks
-export([init/1]).

-define(SIMSUP, ?MODULE).

%% API functions
start_link() -> 
	supervisor:start_link({local, ?SIMSUP}, ?MODULE, []).

%% Supervisor callbacks
%% Child :: {Id, StartFunc, Restart, Shutdown, Type, Modules}
init([]) ->
	 % Max 5 restart within 50ms before supervisor gives up
	 {ok, { {one_for_all, 2, 10}, 
		   [#{id => parser,						% Unique Identifier for child
			 start => {parser, start_link, []},	% Start module with link and [] as Args
			 restart => permanent,				% Permanent process should always be restarted
             shutdown => 3000,					% Set to infinity to give time for sub-sup to shutdown
             type => worker,					% Specify wheter child is worker or supervisor
             modules => [parser]				% Specifing the child's callback module
			},
		   #{id => mqttClientCoordinator,
			 start => {test_client, start_link, []},
             restart => permanent,	
             shutdown => 3000,
             type => worker,
             modules => [test_client] %% Replace with Boyan coordinator
			},
		   #{id => diagramSimulator,
             start => {test_client2, start_link, []},
             restart => permanent,	
             shutdown => infinity,
             type => supervisor,
             modules => [test_client2] %% Replace with Shaun/Elaine 's foshizzle
			}
		  ]}
	 }.
