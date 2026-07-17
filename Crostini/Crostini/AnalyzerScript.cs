//     ______                __  _       _ 
//    / ____/________  _____/ /_(_)___  (_)
//   / /   / ___/ __ \/ ___/ __/ / __ \/ / 
//  / /___/ /  / /_/ (__  ) /_/ / / / / /  
//  \____/_/   \____/____/\__/_/_/ /_/_/   
//        
// Copyright (C) Ukefixed

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
        public Analyzer()
        {
            _configurations = new Dictionary<string, object>();
            _rules = new List<string>();
            _sourcePath = string.Empty;
            _outputPath = "./.crostini/anal_result.ast";
            _enableVerboseLogging = false;
            InitializeDefaultRules();
        }

        /// <summary>
        /// Constructor parameter
        /// </summary>
        public Analyzer(string sourcePath) : this()
        {
            _sourcePath = sourcePath;
        }

        /// <summary>
        /// Constructor with multiple parameters
        /// </summary>
        public Analyzer(string sourcePath, string outputPath) : this(sourcePath)
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
        public Analyzer Configure(string key, object value)
        {
            _configurations[key] = value;
            return this; // Supports chained calls
        }

        /// <summary>
        /// Enable/Disable rule
        /// </summary>
        public Analyzer EnableRule(string ruleName)
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
        public Analyzer DisableRule(string ruleName)
        {
            _rules.Remove(ruleName);
            return this;
        }

        /// <summary>
        /// Source path
        /// </summary>
        public Analyzer SetSourcePath(string path)
        {
            _sourcePath = path;
            return this;
        }

        public Analyzer SetOutputPath(string path)
        {
            _outputPath = path;
            return this;
        }

        /// <summary>
        /// Enable verbose log
        /// </summary>
        public Analyzer EnableVerbose(bool enable = true)
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

            if (!File.Exists(_sourcePath))
            {
                throw new FileNotFoundException($"Source file not found: {_sourcePath}");
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
        IReadOnlyList<Diagnostic> GetDiagnostics();
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
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                if (!File.Exists(filePath))
                {
                    result.Success = false;
                    result.Message = $"File not found: {filePath}";
                    result.Diagnostics = new List<Diagnostic>
                    {
                        new Diagnostic
                        {
                            Rule = "FileSystem",
                            Message = $"File '{filePath}' does not exist.",
                            Line = 0,
                            Severity = DiagnosticSeverity.Error
                        }
                    };
                    return result;
                }

                // 读取源码
                string sourceCode = File.ReadAllText(filePath, Encoding.UTF8);
                var lines = sourceCode.Split(new[] { '\n' }, StringSplitOptions.None);

                if (_enableVerbose)
                {
                    Console.WriteLine($"[Verbose] Analyzing file: {filePath}");
                    Console.WriteLine($"[Verbose] Lines: {lines.Length}");
                    Console.WriteLine($"[Verbose] Rules: {string.Join(", ", _rules)}");
                }

                // 执行规则检查
                foreach (var rule in _rules)
                {
                    ApplyRule(rule, lines, sourceCode);
                }

                // 写入输出文件
                WriteOutput(result);

                result.Success = _diagnostics.All(d => d.Severity != DiagnosticSeverity.Error);
                result.Message = result.Success ? "Analysis completed successfully." : "Analysis completed with errors.";
                result.Diagnostics = new List<Diagnostic>(_diagnostics);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Analysis failed: {ex.Message}";
                result.Diagnostics = new List<Diagnostic>
                {
                    new Diagnostic
                    {
                        Rule = "Runtime",
                        Message = ex.Message,
                        Line = 0,
                        Severity = DiagnosticSeverity.Error
                    }
                };
            }
            finally
            {
                stopwatch.Stop();
                result.ElapsedTime = stopwatch.Elapsed;
            }

            return result;
        }

        private void ApplyRule(string ruleName, string[] lines, string sourceCode)
        {
            switch (ruleName)
            {
                case "TodoComments":
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("TODO") || lines[i].Contains("FIXME"))
                        {
                            _diagnostics.Add(new Diagnostic
                            {
                                Rule = ruleName,
                                Message = "TODO/FIXME comment found.",
                                Line = i + 1,
                                Severity = DiagnosticSeverity.Warning
                            });
                        }
                    }
                    break;

                case "EmptyMethods":
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("void") && lines[i].Contains("()"))
                        {
                            if (i + 1 < lines.Length && lines[i + 1].Trim() == "{}")
                            {
                                _diagnostics.Add(new Diagnostic
                                {
                                    Rule = ruleName,
                                    Message = "Empty method detected.",
                                    Line = i + 1,
                                    Severity = DiagnosticSeverity.Warning
                                });
                            }
                        }
                    }
                    break;

                case "NamingConvention":
                case "CodeComplexity":
                case "UnusedVariables":
                default:
                    if (_enableVerbose)
                    {
                        Console.WriteLine($"[Verbose] Rule '{ruleName}' is not fully implemented yet.");
                    }
                    break;
            }
        }

        private void WriteOutput(AnalysisResult result)
        {
            try
            {
                string? directory = Path.GetDirectoryName(_outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var outputLines = new List<string>
                {
                    "=== Crostini Analysis Result ===",
                    $"Timestamp: {DateTime.Now}",
                    $"Success: {result.Success}",
                    $"Message: {result.Message}",
                    $"Elapsed: {result.ElapsedTime.TotalMilliseconds}ms",
                    "",
                    "=== Diagnostics ==="
                };

                foreach (var diag in _diagnostics)
                {
                    outputLines.Add($"[{diag.Severity}] Line {diag.Line}: {diag.Rule} - {diag.Message}");
                }

                File.WriteAllLines(_outputPath, outputLines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                if (_enableVerbose)
                {
                    Console.WriteLine($"[Verbose] Failed to write output: {ex.Message}");
                }
            }
        }

        public IReadOnlyList<Diagnostic> GetDiagnostics()
        {
            return _diagnostics.AsReadOnly();
        }
    }

    /// <summary>
    /// Diagnose informations
    /// </summary>
    public class Diagnostic
    {
        public string Rule { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Line { get; set; }
        public DiagnosticSeverity Severity { get; set; }
    }

    /// <summary>
    /// Analysis result
    /// </summary>
    public class AnalysisResult
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<Diagnostic> Diagnostics { get; set; } = new();
        public TimeSpan ElapsedTime { get; set; }
    }

    public enum DiagnosticSeverity
    {
        Info,
        Warning,
        Error
    }
}
