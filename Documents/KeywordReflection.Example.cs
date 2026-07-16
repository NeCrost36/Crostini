// Example

using Crostini.KeywordReflection;
using System;
public class Program
{
    var keyType = new KeyType();
    Type ifType = keyType[Keyword.@if];        //Get if type
    Type elseType = keyType.GetTypeFor(Keyword.@else);
}
