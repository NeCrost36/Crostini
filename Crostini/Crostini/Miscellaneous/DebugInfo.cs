using System;
using System.Diagnose;

namespace Crostini
{
    /// <summary>
    /// Debug mode for more information.
    /// </summary>
    public static class Debug
    {
        //Get instance of logger
        var logger = Logger.Instance;
        /// <summary>
        /// Debug Constructor.Probably unused
        /// </summary>
        public void Debug()
        {
            logger.Info(_debugInfo)
            Console.WriteLine(_debugInfo);
        }
        string _debugInfo = "[Crostini] Debug is enabled,";
    }
}
