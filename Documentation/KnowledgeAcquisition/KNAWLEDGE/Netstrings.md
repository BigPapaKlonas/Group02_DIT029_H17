## Netstring
Netstrings are used to avoid complications when passing strings between programming languages and systems through a tcp connection. 
The basic syntax for a net string is: 
[bits.length]:[string],
Example: 32:"hello world", 

When needed encoding can take a string and split each word into: 17:5:hello,6:world!,

Erlang implementation:
https://www.github.com/jdavisp3/netstring
To use it:
clone the repo and use the module:
netstring:encode("abcd").
netstring:decode(<<"4:abcd,">>).

C# implementation is not needed, as I understand netstrings are just .NET string.

This might also be used for parsing an encoded string for faster results and replying it to Unity. The netstrings can also be used as a security measure, even tho this might not really be used by us.

The netstring protocol does not have or need that much information about it, but I think we can use this technology and gain from it in the long run. 
