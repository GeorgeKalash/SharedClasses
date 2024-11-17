
namespace SharedClasses
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public static class FileCompressor
    {
        public static int getProgress(string logFilePath)
        {
            string percent = File.ReadAllText(logFilePath);
            return Convert.ToInt32(percent);
        }

        public static string Compress(string inputFilePath, string outputFilePath)
        {
            string progressLogFilePath = CryptoTools.GenerateUniqueKey();
            File.WriteAllText(progressLogFilePath, "0");
            Task.Run(() =>
            {
                CompressFile(inputFilePath, outputFilePath, progressLogFilePath);
            });


            return progressLogFilePath;
        }
        public static void CompressFile(string inputFilePath, string outputFilePath, string progressLogFilePath)
        {
            const int LOG_PERCENT_INTERVAL = 2;

            if (!File.Exists(inputFilePath))
                throw new FileNotFoundException($"The file '{inputFilePath}' does not exist.");

            const int bufferSize = 8192; // Buffer size for reading the file
            byte[] buffer = new byte[bufferSize];

            int lastProgressPercentage = 0;

            using (FileStream sourceStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream destinationStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            using (GZipStream compressionStream = new GZipStream(destinationStream, CompressionLevel.Optimal))
            {
                long totalBytes = sourceStream.Length;
                long processedBytes = 0;
                int bytesRead;
                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    compressionStream.Write(buffer, 0, bytesRead);
                    processedBytes += bytesRead;
                    int progressPercentage = (int)((processedBytes * 100) / totalBytes);
                    if (progressPercentage > LOG_PERCENT_INTERVAL + lastProgressPercentage)
                    {
                        File.WriteAllText(progressLogFilePath, progressPercentage.ToString());
                        lastProgressPercentage = progressPercentage;
                    }
                }

                File.WriteAllText(progressLogFilePath, "100");

            }
        }
    }

}
