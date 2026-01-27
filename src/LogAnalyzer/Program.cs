using System.Collections.Concurrent;

namespace LogAnalyzer;

internal class Program
{
    static void Main(string[] args)
    {
        var ipCounts = new ConcurrentDictionary<string, long>();

        using var fs = new FileStream(
            "server_logs.txt",
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            1024 * 1024
        );
        using var reader = new StreamReader(fs);

        var lines = new List<string>(100000);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(line);

            if (lines.Count == 100000)
            {
                ProcessBatch(lines, ipCounts);
                lines.Clear();
            }
        }

        if (lines.Count > 0)
            ProcessBatch(lines, ipCounts);

        // TOP 5
        var top5 = ipCounts.OrderByDescending(x => x.Value).Take(5);

        foreach (var ip in top5)
            Console.WriteLine($"{ip.Key} => {ip.Value}");
    }

    static void ProcessBatch(List<string> batch, ConcurrentDictionary<string, long> dict)
    {
        Parallel.ForEach(
            batch,
            line =>
            {
                var parts = line.Split(';');
                var ip = parts[1].Split('=')[1];

                dict.AddOrUpdate(ip, 1, (_, old) => old + 1);
            }
        );
    }
}
