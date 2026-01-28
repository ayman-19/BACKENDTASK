namespace LogAnalyzer;

internal class Program
{
    static void Main(string[] args)
    {
        var analyzer = new LogAnalyzer();
        const string FilePath = "server_logs.txt";
        var result = analyzer.Analyze(FilePath);

        var top5 = result.OrderByDescending(x => x.Value).Take(5);

        foreach (var ip in top5)
            Console.WriteLine($"{ip.Key} => {ip.Value}");

        // summarize this in README
    }
}
