using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace openconfig_yang_tree_view.Services
{
    public static class ParsingService
    {
        public static List<string> Parse(string path)
        {
            List<string> parsedFiles = new List<string>();
            string[] files = Directory.GetFiles(path);
            string yangFilePattern = @"\.yang$";
            foreach (string file in files)
            {
                if (Regex.IsMatch(file, yangFilePattern, RegexOptions.IgnoreCase))
                {
                    try
                    {
                        string yangContents = File.ReadAllText(file);
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        parsedFiles.Add(fileName);


                        Console.WriteLine($"Parsing {fileName}.yang");
                        Console.WriteLine(yangContents);
                        Console.WriteLine($"End of {fileName}.yang\n");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing {file}: {ex.Message}");
                    }
                }
                else
                    continue;
            }
            return parsedFiles;
        }
    }
}
