//Example 1 for analyzer
using Crostini;
public class Program
{
    public static void Main()
    {
        // Use constructor to initialize 
        var analyzer = new Analyzer("C:/Projects/MyApp")
            .SetOutputPath("C:/Reports/analysis.txt")
            .EnableRule("NamingConvention")
            .EnableRule("CodeComplexity")
            .DisableRule("TodoComments")
            .EnableVerbose(true)
            .Configure("MaxComplexity", 10)
            .Build();

        // Analyze
        var result = analyzer.Analyze();

        // Result
        Console.WriteLine($"{result.Message}");
        foreach (var diagnostic in result.Diagnostics)
        {
            Console.WriteLine($"{diagnostic.Rule}: {diagnostic.Message}");
        }
    }
}
