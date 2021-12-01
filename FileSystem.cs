using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Runner
{
    public static class FileSystem
    {
        public static string DataDirectory = @"Data\";

        public static string MakeDataFilePath(string filenamePrefix)
        {
            return $"{DataDirectory}{filenamePrefix}.txt";
        }

        public static IEnumerable<T> Read<T>(this string csvFilePath, Func<string, T> lineConverter)
        {
            using (var reader = new StreamReader(csvFilePath))
            {
                while (!reader.EndOfStream)
                {
                    yield return lineConverter(reader.ReadLine());
                }
            }
        }

        public static IEnumerable<string> Read(this string csvFilePath)
        {
            return csvFilePath.Read(s => s);
        }

        public static IEnumerable<string[]> ReadGroups(this string csvFilePath, Func<string, bool> groupSeparator)
        {
            var allLines = csvFilePath.Read().ToArray();

            var grouped = new List<string[]>();

            var inProgress = new List<string>();

            for (var index = 0; index < allLines.Length; ++index)
            {
                if (!groupSeparator(allLines[index]))
                {
                    inProgress.Add(allLines[index]);
                }
                else
                {
                    grouped.Add(inProgress.ToArray());
                    inProgress.Clear();
                }
            }

            if (inProgress.Any())
            {
                grouped.Add(inProgress.ToArray());
            }

            return grouped;
        }    
    }
}
