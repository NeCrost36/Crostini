//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) NeCrost36
//SyntaxScript
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
namespace Crostini
{
    public class Syntax
    {
        public string result;

        public Syntax()
        {
            result = string.Empty;
        }

        public class Result
        {
            public enum resultType
            {
                //Normal
                success,
                //Have errors
                failed,
                //Means code that has unstable or warning factors
                warning
            }

            public resultType Type { get; set; }
        }
    }

    public class ErrorHandler
    {
        public string errorMessage;
    }
}
