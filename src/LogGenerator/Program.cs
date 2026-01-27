namespace LogGenerator;

internal class Program
{
    static void Main(string[] args)
    {
        const string FilePath = "server_logs.txt";
        const int FileSizeInMB = 500; // 1024 (1GB) 5000 (5GB)
        const int BufferSize = 65536; // 64KB Write Buffer

        Generator.Generate(FilePath, FileSizeInMB, BufferSize);
    }
}
