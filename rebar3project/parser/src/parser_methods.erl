%%%-------------------------------------------------------------------
%%% @author Erik, Justinas
%%% @doc JSON parser
%%%-------------------------------------------------------------------
-module(parser_methods).
-author("Erik and Justinas").

%% API
-export([encode/1, parse_to_map/1, get_SD/0, get_DD/0, get_CD/0, get_diagram/1,
		get_ssd_processes/1, get_class_relationships/1, get_type/1, get_diagram_contents/1,
		get_messages/1, get_classes/1, get_mapping/1, get_Val2/1]).

%% Returns the JSON as an Erlang map without the meta data
parse_to_map(X) -> Z = remove_meta(decode_map(X)), Z.
%%io:format("The ~p map has the following keys: ~p~n~n", [get_type(Z), maps:keys(Z)])

%% Decodes the JSON file into an Erlang map
decode_map(X) ->
  case jsx:is_json(X) of
    true  -> jsx:decode(X, [return_maps]);
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

get_Val2(X) -> {ok, Content} = maps:find(<<"content">>, get_diagram(X)),
  get_Val2_r(Content).

get_Val2_r([]) -> [];
get_Val2_r([X | Y]) -> {ok, Messages} = maps:find(<<"content">>, X),
  [parse_content_list(all_maps_to_list(Messages)) | get_Val2_r(Y)].

parse_content_list([]) -> [];
parse_content_list([H | T]) -> [remove_utf8_encoding(H) | parse_content_list(T)].

all_maps_to_list([]) -> [];
all_maps_to_list([H | T]) -> [maps:to_list(H) | all_maps_to_list(T)].

%% Returns the diagram contents in a list
get_diagram_contents(X) -> case get_type(X) of
                             <<"sequence_diagram">> -> maps:get(<<"content">>, get_diagram(X));
                             _Else -> 'Error, not a sequence diagram'
                           end.

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
    remove_utf8_encoding(T)].

%% Returns the processes in a list
get_ssd_processes(X) -> case get_type(X) of
                      <<"diagram">> -> maps:get(<<"processes">>, X);
                      _Else -> 'Error, not a sequence diagram'
                    end.

%% Returns the messages in a list
get_messages(X) -> case get_type(X) of
                     <<"content">> -> maps:get(<<"message">>, X);
                     _Else -> 'Error, message'
                    end. 

%% Returns the classes from CD
get_classes(X) -> case get_type(X) of
                    <<"class_diagram">> -> maps:get(<<"classes">>, X);
                    _Else -> 'Error, not a class diagram'
                  end.

%% Returns the relationships in a list
get_class_relationships(X) -> case get_type(X) of
                          <<"class_diagram">> -> maps:get(<<"relationships">>, X);
                          _Else -> 'Error, not a class diagram'
                        end.

%% Returns the diagram type
get_mapping(X) -> case get_type(X) of
                    <<"deployment_diagram">> -> maps:get(<<"mapping">>, X);
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
  {ok, File} = file:read_file("CD.json"), parse_to_map(File).

%% Returns a JSON deployment diagram
get_DD() ->
  {ok, File} = file:read_file("DD.json"), parse_to_map(File).