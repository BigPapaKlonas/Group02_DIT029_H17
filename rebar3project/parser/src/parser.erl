-module(parser).
-export([encode/1, parse_to_map/1, get_json/0, get_diagram/1, get_processes/1, get_classes/1, get_relationships/1,
  get_type/1, get_mapping/1]).

    %% Returns the JSON as an Erlang map without the meta data.
    parse_to_map(X) -> Z = remove_meta(decode_map(X)), io:format("The ~p map has the following keys: ~p~n~n",
      [get_type(Z), maps:keys(Z)]), Z.

    %% Decodes the JSON file into an Erlang map"
    decode_map(X) ->
      case jsx:is_json(X) of
        true  -> jsx:decode(X, [return_maps]);
        false -> 'not a valid JSON'
      end.

    %% Removes potential meta data from decoded JSON %%
    remove_meta(X) -> maps:remove(<<"meta">>, X).

    %% Returns what the keys contain %%
    get_type(X) -> maps:get(<<"type">>, X).
    get_diagram(X) -> maps:get(<<"diagram">>, X).
    get_processes(X) -> maps:get(<<"processes">>, X).
    get_classes(X) -> maps:get(<<"classes">>, X).
    get_relationships(X) -> maps:get(<<"relationships">>, X).
    get_mapping(X) -> maps:get(<<"mapping">>, X).

    %% Converts Erlang binary map into JSON string
    encode(X) ->
      io:format("Encoding to JSON string ~p~n", ['...']), jsx:encode(X).

    %% Returns JSON test file %%
    get_json() ->
      {ok, File} = file:read_file("diagramdata.json"), File.