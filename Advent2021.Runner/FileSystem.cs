using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Runner
{
    public static class FileSystem
    {
        public static string DataDirectory = @"Data\";

        /// <summary>
        /// Returns the relative path to the text file whose name (without extension) is filenamePrefix.
        /// </summary>
        /// <param name="filenamePrefix"></param>
        /// <returns></returns>
        public static string MakeDataFilePath(string filenamePrefix)
        {
            return $"{DataDirectory}{filenamePrefix}.txt";
        }

        /// <summary>
        /// Returns each line in csvFilePath after it's processed by lineConverter. 
        /// Evaluation is lazy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvFilePath"></param>
        /// <param name="lineConverter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Reads and returns each line from csvFilePath, as-is. 
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <returns></returns>
        public static IEnumerable<string> Read(this string csvFilePath)
        {
            return csvFilePath.Read(s => s);
        }

        /// <summary>
        /// Returns the contents of csvFilePath grouped into separate string arrays based on
        /// detecting a separator via groupSeparator. File read is greedy and processed to the end before returned. 
        /// </summary>
        /// <param name="csvFilePath"></param>
        /// <param name="groupSeparator"></param>
        /// <returns></returns>
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
