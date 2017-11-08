%%%-------------------------------------------------------------------
%%% @author Erik, Justinas
%%% @doc JSON parser
%%%-------------------------------------------------------------------
-module(parser_methods).
-author("Erik and Justinas").

%% API
-export([encode/1, parse_to_map/1, get_SD/0, get_DD/0, get_CD/0,
  get_diagram/1, get_processes/1, get_relationships/1, get_type/1,
  get_classes/1, get_mapping/1, get_messages/1, decode_list/1, parse_to_list/1]).

%% Returns the JSON as an Erlang map without the meta data
parse_to_map(X) -> Z = remove_meta(decode_map(X)), Z.
%%io:format("The ~p map has the following keys: ~p~n~n", [get_type(Z), maps:keys(Z)])

%% Decodes the JSON file into an Erlang map
decode_map(X) ->
  case jsx:is_json(X) of
    true  -> jsx:decode(X, [return_maps]);
    false -> 'not a valid JSON'
  end.

%% Returns the JSON as an Erlang map without the meta data
parse_to_list(X) ->decode_list(X).
%%io:format("The ~p map has the following keys: ~p~n~n", [get_type(Z), maps:keys(Z)])

%% Decodes the JSON file into an Erlang map
decode_list(X) ->
  case jsx:is_json(X) of
    true  -> jsx:decode(X);
    false -> 'not a valid JSON'
  end.

%% Removes potential meta data from decoded JSON, if no meta present, just returns X
remove_meta(X) -> maps:remove(<<"meta">>, X).

%% Returns the diagram type
get_type(X) -> maps:get(<<"type">>, X).

%% Returns the diagram
get_diagram(X) -> case get_type(X) of
                    <<"sequence_diagram">> -> maps:get(<<"diagram">>, X);
                    _Else -> 'Error, not a sequence diagram'
                  end.

get_messages(X) -> {ok, Content} = maps:find(<<"content">>, get_diagram(X)),
  parse_content(Content).

parse_content([]) -> [];
parse_content([X | Y]) -> {ok, Messages} = maps:find(<<"content">>, X),
  [parse_list(all_maps_to_list(Messages)) | parse_content(Y)].

parse_classes([]) -> [];
parse_classes([H | T]) -> [parse_fields(H) | parse_classes(T)].

parse_list([]) -> [];
parse_list([H | T]) -> [remove_utf8_encoding(H) | parse_list(T)].

all_maps_to_list([]) -> [];
all_maps_to_list([H | T]) -> [maps:to_list(H) | all_maps_to_list(T)].

remove_utf8_encoding([]) -> [];
remove_utf8_encoding([{F, S} | T]) when is_list(S) ->
  [{unicode:characters_to_list(F, utf8),
    remove_utf8_encoding(S)} |
    remove_utf8_encoding(T)];
remove_utf8_encoding([{F, S} | T]) ->
  [{unicode:characters_to_list(F, utf8),
  unicode:characters_to_list(S, utf8)} |
    remove_utf8_encoding(T)];
remove_utf8_encoding([H | T]) ->
  [unicode:characters_to_list(H, utf8) |
    remove_utf8_encoding(T)];
remove_utf8_encoding({H, T}) ->
  [{unicode:characters_to_list(H, utf8),
    unicode:characters_to_list(T, utf8)}].

remove_utf8_class([]) -> [];
remove_utf8_class([L]) -> remove_utf8_class(L);
remove_utf8_class([H | T]) ->
  [remove_utf8_class(H),
    remove_utf8_class(T)];
remove_utf8_class({H, T}) ->
  {unicode:characters_to_list(H, utf8),
    unicode:characters_to_list(T, utf8)}.

%% Returns the processes in a list
get_processes(X) -> case get_type(X) of
                      <<"sequence_diagram">> -> parse_list(
                        all_maps_to_list(maps:get(<<"processes">>, X)));
                      _Else -> 'Error, not a sequence diagram'
                    end.

parse_fields([{NameKey, Name}, {Field, List}]) ->
  [{unicode:characters_to_list(NameKey),
    unicode:characters_to_list(Name)}, {unicode:characters_to_list(Field),
    remove_utf8_class(List)}].

%% Returns the classes from CD
get_classes(X) -> parse_classes(proplists:get_value(<<"classes">>, X)).

%% Returns the relationships in a list
get_relationships(X) -> parse_list(proplists:get_value(<<"relationships">>, X)).


%% Returns the diagram type
get_mapping(X) -> case get_type(X) of
                    <<"deployment_diagram">> -> parse_list(
                      all_maps_to_list(maps:get(<<"mapping">>, X)));
                    _Else -> 'Error, not a deployment diagram'
                  end.


%% Converts Erlang binary map into JSON string
encode(X) ->
  io:format("Encoding to JSON string ~p~n", ['...']), jsx:encode(X).

%% Returns a JSON sequence diagram
get_SD() ->
  {ok, File} = file:read_file("triparallel.json"), parse_to_map(File).

%% Returns a JSON class diagram
get_CD() ->
  {ok, File} = file:read_file("CD.json"), parse_to_list(File).

%% Returns a JSON deployment diagram
get_DD() ->
  {ok, File} = file:read_file("DD.json"), parse_to_map(File).