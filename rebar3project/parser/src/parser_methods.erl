%%%-------------------------------------------------------------------
%%% @author Erik, Justinas
%%% @doc JSON parser
%%%-------------------------------------------------------------------
-module(parser_methods).
-author("Erik and Justinas").

%% API
-export([encode/1, get_SD/0, get_DD/0, get_CD/0, get_type/1,
  get_diagram/1, get_parsed_diagram/1, get_processes/1,
  get_relationships/1, get_classes/1, get_mapping/1, get_messages/1]).

%% Decodes the JSON file into an Erlang map
decode_map(X) ->
  case jsx:is_json(X) of
    true  -> jsx:decode(X, [return_maps]);
    false -> 'not a valid JSON'
  end.

%% Decodes the JSON file into an Erlang list
decode_list(X) ->
  case jsx:is_json(X) of
    true  -> jsx:decode(X);
    false -> 'not a valid JSON'
  end.

%% Returns the diagram type
get_type(X) -> maps:get(<<"type">>, X).

%% Returns the diagram from JSON with SSD data
get_diagram(X) -> case get_type(X) of
                    <<"sequence_diagram">> -> maps:get(<<"diagram">>, X);
                    _Else -> 'Error, not a sequence diagram'
                  end.

% Returns parsed data from binary JSON
get_parsed_diagram(X) ->
  % Parse binary JSON to map in order to check for what kind of diagram it is
  Z = decode_map(X),
  % Check the type of diagram in JSON
  case get_type(Z) of
    <<"sequence_diagram">> -> [get_messages(Z) | [get_processes(Z)]];
    <<"deployment_diagram">> -> get_mapping(Z);
    % Parse CD to list, because it's easier to work with it that way
    <<"class_diagram">> -> Y = decode_list(X),
      [get_classes(Y) | [get_relationships(Y)]];
    _Else -> 'Diagram type not supported'
  end.

% Return parsed messages from SSD JSON
get_messages(X) ->
  % Find map with the "content" key
  {ok, Content} = maps:find(<<"content">>, get_diagram(X)),
  % parse content
  parse_content(Content).

%% Returns the parsed process from SSD
get_processes(X) -> case get_type(X) of
                      <<"sequence_diagram">> ->
                        % Remove the binary encoding from the list
                        parse_list(
                          % Convert the maps to list
                          maps_to_list(
                            % Get the "processes" map from JSON
                            maps:get(<<"processes">>, X)));
                      _Else -> 'Error, not a sequence diagram'
                    end.

%% Returns the classes from CD
get_classes(X) ->
  % Remove the binary encoding with a pattern matching function
  iterate_classes(
    % Get the list of "classes" from JSON
    proplists:get_value(<<"classes">>, X)).

%% Returns the class relationships from CD
get_relationships(X) ->
  % Remove the binary encoding from the list
  parse_list(
    % Get the list of "relationships" from JSON
    proplists:get_value(<<"relationships">>, X)).


%% Returns the device mapping from DD
get_mapping(X) -> case get_type(X) of
                    <<"deployment_diagram">> ->
                      % Remove the binary encoding from the list
                      parse_list(
                        % Convert the maps to list
                        maps_to_list(
                          % Get the "mapping" map from JSON
                          maps:get(<<"mapping">>, X)));
                    _Else -> 'Error, not a deployment diagram'
                  end.

% Traverses all the different sub-contents of SDD while parsing them
% Reason for the method to being separate is to find the sub-contents
parse_content([]) -> [];
parse_content([X | Y]) ->
  % Find the sub-contents from JSON, project description says there will be at least one
  {ok, Messages} = maps:find(<<"content">>, X),
  % Turn the sub content map into a list and parse it before making a recursive call to the next sub-content
  [parse_list(maps_to_list(Messages)) | parse_content(Y)].

% Traverses all the different classes in CD and calls a function to parse them with pattern matching
iterate_classes([]) -> [];
iterate_classes([H | T]) -> [parse_fields(H) | iterate_classes(T)].

% Parses the sub-fields within CD JSON
parse_fields([{NameKey, Name}, {Field, FieldList}]) ->
  % Remove binary utf8 encoding from the values
  [{unicode:characters_to_list(NameKey), unicode:characters_to_list(Name)},
    {unicode:characters_to_list(Field),
      % Call a function to parse the different elements within the Field List
      remove_utf8_class(FieldList)}].

% Traverses lists and calls a function to remove binary encoding for the Head
parse_list([]) -> [];
parse_list([H | T]) -> [remove_utf8(H) | parse_list(T)].

% Changes the top level maps to lists
maps_to_list([]) -> [];
maps_to_list([H | T]) -> [maps:to_list(H) | maps_to_list(T)].

% Removes binary utf8 encoding from DD and SSD
remove_utf8([]) -> [];
% Removes the utf8 encoding before making a recursive call to the tail
remove_utf8([{F, S} | T]) ->
  [{unicode:characters_to_list(F, utf8),
    unicode:characters_to_list(S, utf8)} |
    remove_utf8(T)].

% Removes binary utf8 encoding from CD
remove_utf8_class([]) -> [];
% The fields come in in 2 nested lists, clause is used to remove the fields(L) from the top layer list
% This interferes with the SSD and DD diagrams, hence the separate method
remove_utf8_class([L]) -> remove_utf8_class(L);
% Clause used to iterate through
remove_utf8_class([H | T]) ->
  % Recursive call to parse the Field list
  [remove_utf8_class(H),
    % Recursive call to parse the next Field list
    remove_utf8_class(T)];
% Clause used to remove utf8 binary encoding
remove_utf8_class({H, T}) ->
  {unicode:characters_to_list(H, utf8),
    unicode:characters_to_list(T, utf8)}.

%% Converts Erlang binary map into JSON string
encode(X) ->
  io:format("Encoding to JSON string ~p~n", ['...']), jsx:encode(X).

%% Returns a JSON sequence diagram
get_SD() ->
  {ok, File} = file:read_file("triparallel.json"), File.

%% Returns a JSON class diagram
get_CD() ->
  {ok, File} = file:read_file("CD.json"), File.

%% Returns a JSON deployment diagram
get_DD() ->
  {ok, File} = file:read_file("DD.json"), File.