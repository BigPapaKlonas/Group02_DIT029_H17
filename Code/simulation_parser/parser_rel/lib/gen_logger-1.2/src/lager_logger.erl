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

-module(lager_logger).

-author("Feng Lee <feng@emqtt.io>").

-behaviour(gen_logger).

-export([debug/1, debug/2,
         info/1, info/2,
         warning/1, warning/2,
         error/1, error/2,
         critical/1, critical/2]).

%%%=========================================================================
%%% lagger logger
%%%=========================================================================

debug(Msg) ->
    lager:debug(Msg).

debug(Format, Args) ->
    lager:debug(Format, Args).

info(Msg) ->
    lager:info(Msg).

info(Format, Args) ->
    lager:info(Format, Args).

warning(Msg) ->
    lager:warning(Msg).

warning(Format, Args) ->
    lager:warning(Format, Args).

error(Msg) ->
    lager:error(Msg).

error(Format, Args) ->
    lager:error(Format, Args).

critical(Msg) ->
    lager:critical(Msg).

critical(Format, Args) ->
    lager:critical(Format, Args).

