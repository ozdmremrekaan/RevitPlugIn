using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitPlugIn.Utils
{
    public class GeneralUtils
    {
        public static string TemporaryFolderPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Factory Reality\FRInteroperabilityTools\Temporary\");
        public static string DecompressTextFile(string compressedFilePath)
        {
            using (FileStream compressedFileStream = File.OpenRead(compressedFilePath))
            using (MemoryStream decompressedMemoryStream = new MemoryStream())
            using (GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(decompressedMemoryStream);
                byte[] decompressedBytes = decompressedMemoryStream.ToArray();
                return Encoding.UTF8.GetString(decompressedBytes);
            }
        }

        public static void CompressTextString(string inputText, string compressedFilePath)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputText);

            using (FileStream compressedFileStream = File.Create(compressedFilePath))
            using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
            {
                compressionStream.Write(inputBytes, 0, inputBytes.Length);
                compressionStream.Flush(); // Ensure all data is flushed to the compressedFileStream
            }
        }

    }
}
