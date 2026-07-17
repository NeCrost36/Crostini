//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) Ukefixed
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
//S0rry too lazy for added error message.
//i havent do this yet
//It's work in progress
namespace Crostini
{
    ///<summary>
    ///Error function
    ///<summary>
    public class Error
    {
        public Error()
        {
            Syntax.ResultType resultType = Syntax.ResultType.success;

            switch (resultType)
            {
                case Syntax.ResultType.success:
                    break;
                case Syntax.ResultType.warning:
                    break;
                case Syntax.ResultType.failed:
                    Console.WriteLine(errorHandler.errorMessage);
                    break;
            }
        }
    }
}
