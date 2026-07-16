//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
//Copyright (C) NeCrost36
using System;
using System.Collections.Generic;

namespace Crostini
{
    /// <summary>
    /// AnalyzeScript : Initialize and Enable/Disable rules
    /// </summary>
    public class Analyzer
    {
        private readonly Dictionary<string, object> _configurations;
        private readonly List<string> _rules;
        private string _sourcePath;
        private string _outputPath;
        private bool _enableVerboseLogging;

        /// <summary>
        /// Initialize
        /// </summary>
        public AnalyzerScript()
        {
            _configurations = new Dictionary<string, object>();
            _rules = new List<string>();
            _sourcePath = string.Empty;
            _outputPath = "analysis_result.txt";
            _enableVerboseLogging = false;
            InitializeDefaultRules();
        }

        /// <summary>
        /// Constructor parameter
        /// </summary>
        public AnalyzerScript(string sourcePath) : this()
        {
            _sourcePath = sourcePath;
        }

        /// <summary>
        /// Constructor with multiple parameters
        /// </summary>
        public AnalyzerScript(string sourcePath, string outputPath) : this(sourcePath)
        {
            _outputPath = outputPath;
        }

        /// <summary>
        /// Initialize default rules
        /// </summary>
        private void InitializeDefaultRules()
        {
            _rules.AddRange(new[]
            {
                "NamingConvention",
                "CodeComplexity",
                "UnusedVariables",
                "EmptyMethods",
                "TodoComments"
            });
        }

        /// <summary>
        /// Configure parser
        /// </summary>
        public AnalyzerScript Configure(string key, object value)
        {
            _configurations[key] = value;
            return this; // Supports chained calls
        }

        /// <summary>
        /// Enable/Disable rule
        /// </summary>
        public AnalyzerScript EnableRule(string ruleName)
        {
            if (!_rules.Contains(ruleName))
            {
                _rules.Add(ruleName);
            }
            return this;
        }

        /// <summary>
        /// Disable rule
        /// </summary>
        public AnalyzerScript DisableRule(string ruleName)
        {
            _rules.Remove(ruleName);
            return this;
        }

        /// <summary>
        /// Source path
        /// </summary>
        public AnalyzerScript SetSourcePath(string path)
        {
            _sourcePath = path;
            return this;
        }

        public AnalyzerScript SetOutputPath(string path)
        {
            _outputPath = path;
            return this;
        }

        /// <summary>
        /// Enable verbose log
        /// </summary>
        public AnalyzerScript EnableVerbose(bool enable = true)
        {
            _enableVerboseLogging = enable;
            return this;
        }

        /// <summary>
        /// Create a parser instance
        /// </summary>
        public IAnalyzer Build()
        {
            ValidateConfiguration();

            // Create a parser instance
            return new CodeAnalyzer(
                _sourcePath,
                _outputPath,
                _rules,
                _configurations,
                _enableVerboseLogging
            );
        }

        /// <summary>
        /// Check if the validation configurations are complete
        /// </summary>
        private void ValidateConfiguration()
        {
            if (string.IsNullOrEmpty(_sourcePath))
            {
                throw new InvalidOperationException("Source path not set");
            }

            if (_rules.Count == 0)
            {
                throw new InvalidOperationException("At least one analysis rule must be enabled.");
            }
        }
    }

    /// <summary>
    /// Analyzer interface
    /// </summary>
    public interface IAnalyzer
    {
        AnalysisResult Analyze();
        AnalysisResult Analyze(string filePath);
        List<Diagnostic> GetDiagnostics();
    }

    /// <summary>
    /// Analyzer achieve
    /// </summary>
    public class CodeAnalyzer : IAnalyzer
    {
        private readonly string _sourcePath;
        private readonly string _outputPath;
        private readonly List<string> _rules;
        private readonly Dictionary<string, object> _configurations;
        private readonly bool _enableVerbose;
        private readonly List<Diagnostic> _diagnostics;

        public CodeAnalyzer(
            string sourcePath,
            string outputPath,
            List<string> rules,
            Dictionary<string, object> configurations,
            bool enableVerbose)
        {
            _sourcePath = sourcePath;
            _outputPath = outputPath;
            _rules = rules;
            _configurations = configurations;
            _enableVerbose = enableVerbose;
            _diagnostics = new List<Diagnostic>();
        }

        public AnalysisResult Analyze()
        {
            return Analyze(_sourcePath);
        }

        public AnalysisResult Analyze(string filePath)
        {
            var result = new AnalysisResult();
            return result;
        }

        public List<Diagnostic> GetDiagnostics()
        {
            return _diagnostics;
        }
    }

    /// <summary>
    /// Diagnose informations
    /// </summary>
    public class Diagnostic
    {
        public string Rule { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }
        public DiagnosticSeverity Severity { get; set; }
    }

    /// <summary>
    /// Analysis result
    /// </summary>
    public class AnalysisResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Diagnostic> Diagnostics { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }

    public enum DiagnosticSeverity
    {
        Info,
        Warning,
        Error
    }

}
