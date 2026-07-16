//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) NeCrost36
using System;
using System.Collections.Generic;
//Using Linq syntax to be more...lightweight?Or what-
using System.Linq;

public class KeywordReflection
{
    //Generate all Dictionary on runtime
    public Dictionary<Keyword, Type> TypeMap { get; }

    public KeywordReflection()
    {
        TypeMap = Enum.GetValues(typeof(Keyword))
            .Cast<Keyword>()
            .ToDictionary(
                key => key,
                value => Type.GetType($"System.{value}") ?? typeof(object)
            );
    }
    // Reflection 
    public Type GetTypeFor(Keyword key) => TypeMap[key];
}


