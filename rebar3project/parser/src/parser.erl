-module(parser).
-behaviour(gen_server).
-export([start_link/0, stop/0, crash/0, start_wait/0, terminate/2, parse/1, init/1, handle_call/3]).

start_link() ->
  gen_server:start_link({local, ?MODULE}, ?MODULE, [], []).

init(_Args) -> {ok, []}.

start_wait() ->
  io:format("What kinda JSON do you want to load (SD, DD or CD)?~n"),
  io:format("Enter \'quit\' to exit.~n"),
  display_prompt(),
  receive_loop().

handle_call({parse, Path}, {Pid, _Ref}, State) ->
  spawn_parse_process(Path, Pid),
  {reply, ok, State};
handle_call(crash, _, _) -> error(crash);
handle_call(stop, _, State) ->
  io:format("stopping!~n"),
  {stop, terminated, State}.

terminate(_Reason, _State) -> ok.

parse(M) -> gen_server:call(?MODULE, {parse, M}).

crash() -> gen_server:call(?MODULE, crash).

stop() -> gen_server:call(?MODULE, stop).

%% Spawns a new process for every parsing job received
spawn_parse_process(Path, Pid) ->
  spawn(fun () ->
    M = parser_methods:parse_to_map(load_file(Path)),
    Pid ! {parsed, M, {self(), make_ref()}}
        end),
  ok.

%% for debugging purpose, going to be replaced by the actual communication protocol
load_file(Path) ->
  case Path of
    "SD\n"    -> D = "SD.json";
    "DD\n"    -> D = "DD.json";
    "CD\n"    -> D = "CD.json";
    _Else     -> D = "CD.json"
  end,
  {ok, File} = file:read_file(D), File.

% The prompt is handled by a separate process.
% Every time a message is entered (and io:get_line()
% returns) the separate process will send a message
% to the main process (and terminate).
display_prompt() ->
  Client = self(),
  spawn(fun () ->
    M = io:get_line("> "),
    Client ! {parse, M, self(), make_ref()}
        end),
  ok.

%% Loop that keeps the receive statement going
receive_loop() ->
  receive
    {parsed, M, {Pid, Ref}}        ->
      io:format("JSON parsed on PID: ~p with REF: ~p~n", [Pid, Ref]),
      io:format("~p~n", [M]),
      receive_loop();
    {parse, "quit\n", _Pid, _Ref} ->
      ok;
    {parse, M, _Pid, _Ref}        ->
      parse(M),
      display_prompt(),
      receive_loop()
  end.