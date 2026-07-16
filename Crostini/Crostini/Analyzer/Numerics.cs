//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) NeCrost36
using System;
using CrostiniAnalyzer;

namespace Crostini
{
    public class Numerics : AnalyzerScript
    {
        public enum NumericsType
        {
            @int,
            @nint,
            @uint,
            @float,
            //Feature? idk
            @nuint,
            @double
        }
    }
}
