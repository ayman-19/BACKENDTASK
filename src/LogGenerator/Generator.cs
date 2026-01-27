using System.Diagnostics;
using System.Text;

namespace LogGenerator;

public static class Generator
{
    public static void Generate(string filePath, int fileSizeInMB, int bufferSize)
    {
        Console.WriteLine($"Generating {fileSizeInMB}MB log file at: {filePath}...");

        var random = new Random();

        var ipPool = new List<string>();
        for (int i = 0; i < 50; i++)
            ipPool.Add($"192.168.1.{i}");
        for (int i = 0; i < 50; i++)
            ipPool.Add($"10.0.0.{i}");

        long totalBytesWritten = 0;
        long targetBytes = (long)fileSizeInMB * 1024 * 1024;

        var stopwatch = Stopwatch.StartNew();

        using (var writer = new StreamWriter(filePath, false, Encoding.UTF8, bufferSize))
        {
            while (totalBytesWritten < targetBytes)
            {
                int index = (int)(Math.Pow(random.NextDouble(), 3) * ipPool.Count);
                string ip = ipPool[index];

                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                int duration = random.Next(10, 5000); // 10ms to 5s
                string path = $"/api/v1/resource/{random.Next(1, 100)}";

                string line = $"timestamp={timestamp};ip={ip};duration={duration}ms;path={path}";

                writer.WriteLine(line);
                totalBytesWritten += line.Length + 2;
            }
        }

        stopwatch.Stop();
        Console.WriteLine(
            $"\nDone. File generated in {stopwatch.Elapsed.TotalSeconds:F2} seconds."
        );
    }
}
