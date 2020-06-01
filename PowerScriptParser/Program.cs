using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PowerScriptParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            var fullPath = @"d:\MyUtilities\PowerScriptParser\_Sources\w_cladmin_subscriber\w_cladmin_subscriber.srw";

            var lines = File.ReadAllLines(fullPath);

            var path = Path.GetDirectoryName(fullPath);

            GetSubMethods(lines, path, "su_alternate_subs_id");

            //FindAllEntrances(lines, @"cbd_renewal_mnth");

            Console.ReadKey();
        }

        private static void GetSubMethods(string[] lines, string path, string text)
        {
            var lineNumbers = Helper.GetLineNumbers(lines, text);

            var numbers = new List<Tuple<int, int>>();

            foreach (var number in lineNumbers)
            {
                if (!Helper.TryGetStartNum(number, lines, out var startNumber) ||
                    !Helper.TryGetEndNum(number, lines, out var endNumber))
                {
                    continue;
                }

                var value = new Tuple<int, int>(startNumber, endNumber);
                if (numbers.Contains(value))
                {
                    continue;
                }

                numbers.Add(value);
                var subLines = lines.Skip(startNumber).Take(endNumber - startNumber + 1);

                var directoryPath = $@"{path}\methods";
                Directory.CreateDirectory(directoryPath);

                var filePath = $@"{path}\methods\{number}.srw";
                File.WriteAllLines(filePath, subLines);

                Console.WriteLine($"Created new file: {filePath}");
            }

            Console.WriteLine("End.");
        }



        private static void FindAllEntrances(string[] lines, string text)
        {
            var lineNumbers = Helper.GetLineNumbers(lines, text, line => !line.Trim().StartsWith("/"));

            foreach (var number in lineNumbers)
            {
                Console.WriteLine($"{number + 1} : {lines.ElementAt(number).Trim()}");
            }
        }
    }
}
