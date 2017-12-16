%%%-----------------------------------------------------------------------------
%%% Copyright (c) 2014-2016 Feng Lee <feng@emqtt.io>. All Rights Reserved.
%%%
%%% Permission is hereby granted, free of charge, to any person obtaining a copy
%%% of this software and associated documentation files (the "Software"), to deal
%%% in the Software without restriction, including without limitation the rights
%%% to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
%%% copies of the Software, and to permit persons to whom the Software is
%%% furnished to do so, subject to the following conditions:
%%%
%%% The above copyright notice and this permission notice shall be included in all
%%% copies or substantial portions of the Software.
%%%
%%% THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
%%% IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
%%% FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
%%% AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
%%% LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
%%% OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
%%% SOFTWARE.
%%%-----------------------------------------------------------------------------

-module(console_logger).

-author("Feng Lee <feng@emqtt.io>").

-import(lists, [concat/1]).

-behaviour(gen_logger).

-export([debug/1, debug/2,
         info/1, info/2,
         warning/1, warning/2,
         error/1, error/2,
         critical/1, critical/2]).

%%%=========================================================================
%%% io:format
%%%=========================================================================

debug(Msg) ->
    io:format(concat(["[debug] ", Msg, "\n"])).

debug(Format, Args) ->
    io:format(concat(["[debug] ", Format, "\n"]), Args).

info(Msg) ->
    io:format(concat(["[info] ", Msg, "\n"])).

info(Format, Args) ->
    io:format(concat(["[info] ", Format, "\n"]), Args).

warning(Msg) ->
    io:format(concat(["[warning] ", Msg, "\n"])).

warning(Format, Args) ->
    io:format(concat(["[warning] ", Format, "\n"]), Args).

error(Msg) ->
    io:format(concat(["[error] ", Msg, "\n"])).

error(Format, Args) ->
    io:format(concat(["[error] ", Format, "\n"]), Args).

critical(Msg) ->
    io:format(concat(["[critical] ", Msg, "\n"])).

critical(Format, Args) ->
    io:format(concat(["[critical] ",Format, "\n"]), Args).


