using System.Collections.Concurrent;

namespace LogAnalyzer;

public class LogAnalyzer
{
    private readonly int _batchSize;

    public LogAnalyzer(int batchSize = 100000)
    {
        _batchSize = batchSize;
    }

    public IDictionary<string, long> Analyze(string filePath)
    {
        var ipCounts = new ConcurrentDictionary<string, long>();

        using var fs = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 1024 * 1024
        );

        using var reader = new StreamReader(fs);

        var lines = new List<string>(_batchSize);
        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(line);

            if (lines.Count == _batchSize)
            {
                ProcessBatch(lines, ipCounts);
                lines.Clear();
            }
        }

        if (lines.Count > 0)
            ProcessBatch(lines, ipCounts);

        return ipCounts;
    }

    private static void ProcessBatch(List<string> batch, ConcurrentDictionary<string, long> dict)
    {
        Parallel.ForEach(
            batch,
            line =>
            {
                var parts = line.Split(';');
                if (parts.Length < 2)
                    return;

                var ipPart = parts[1].Split('=');
                if (ipPart.Length < 2)
                    return;

                var ip = ipPart[1];

                dict.AddOrUpdate(ip, 1, (_, old) => old + 1);
            }
        );
    }
}
