%%%-------------------------------------------------------------------
%% @doc pubsub_ssdchat top level supervisor.
%% @end
%%%-------------------------------------------------------------------

-module(pubsub_ssdchat_sup).

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
		{pubsub_ssdchat, start_link, []},
		permanent, 1000, worker, [pubsub_ssdchat]}
		]}}.

%%====================================================================
%% Internal functions
%%====================================================================
