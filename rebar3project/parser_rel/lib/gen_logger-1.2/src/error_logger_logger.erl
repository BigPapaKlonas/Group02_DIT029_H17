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

-module(error_logger_logger).

-author("Feng Lee <feng@emqtt.io>").

-behaviour(gen_logger).

-export([debug/1, debug/2,
         info/1, info/2,
         warning/1, warning/2,
         error/1, error/2,
         critical/1, critical/2]).

%%%=========================================================================
%%% error logger
%%%=========================================================================

debug(Msg) ->
    error_logger:info_msg(["[debug] " | Msg]).

debug(Format, Args) ->
    error_logger:info_msg(["[debug] " | Format], Args).

info(Msg) ->
    error_logger:info_msg(["[info] " | Msg]).

info(Format, Args) ->
    error_logger:info_msg(["[info] " | Format], Args).

warning(Msg) ->
    error_logger:warning_msg(["[warning] " | Msg]).

warning(Format, Args) ->
    error_logger:warning_msg(["[warning] " | Format], Args).

error(Msg) ->
    error_logger:error_msg(["[error] " | Msg]).

error(Format, Args) ->
    error_logger:error_msg(["[error] " | Format], Args).

critical(Msg) ->
    error_logger:error_msg(["[critical] " | Msg]).

critical(Format, Args) ->
    error_logger:error_msg(["[critical] " | Format], Args).

