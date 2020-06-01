using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PowerScriptParser
{
    public static class Helper
    {
        public static List<int> GetLineNumbers(string[] lines, string text)
        {
            return GetLineNumbers(lines, text, _ => true);
        }

        public static List<int> GetLineNumbers(string[] lines, string text, Func<string, bool> condition)
        {
            var lineNumbers = new List<int>();
            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                if (Regex.IsMatch(line, text, RegexOptions.IgnoreCase) && condition(line))
                {
                    lineNumbers.Add(index);
                }
            }

            return lineNumbers;
        }

        public static bool TryGetStartNum(int number, string[] lines, out int startNumber)
        {
            startNumber = 0;
            for (var i = number; i > 0; i--)
            {
                var line = lines[i];
                if (!Regex.IsMatch(line, @"^(public|private)\s+function\s") &&
                    !Regex.IsMatch(line, @"^(public|private)\s+subroutine\s") &&
                    !Regex.IsMatch(line, @"^event\s+"))
                {
                    continue;
                }

                startNumber = i;
                return true;
            }

            return false;
        }

        public static bool TryGetEndNum(int number, string[] lines, out int endNumber)
        {
            endNumber = 0;
            for (var i = number; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!Regex.IsMatch(line, @"^end\s+function") &&
                    !Regex.IsMatch(line, @"^end\s+subroutine") &&
                    !Regex.IsMatch(line, @"^end\s+event"))
                {
                    continue;
                }

                endNumber = i;
                return true;
            }

            return false;
        }
    }
}
