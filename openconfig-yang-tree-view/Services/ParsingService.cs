using openconfig_yang_tree_view.InMemoryDB;
using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace openconfig_yang_tree_view.Services
{
    public static class ParsingService
    {
        private static string _folderPath = string.Empty;
        public static List<string> MissingFiles { get; set; } = new List<string>();
        public static List<string> ParseFromFolder(string path)
        {
            _folderPath = path;
            List<string> parsedFiles = new List<string>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (file.EndsWith(".yang"))
                {
                    try
                    {
                        string yangContent = File.ReadAllText(file);
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        Console.WriteLine($"Parsing {fileName}.yang");
                        yangContent = yangContent.TrimAllLines();
                        ParseFile(yangContent);
                        Console.WriteLine($"End of {fileName}.yang\n");
                        parsedFiles.Add(fileName);
                        Console.WriteLine(YangRepository._dataBase.Modules.Count);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} \n {ex.InnerException}");
                    }
                }
                else
                    continue;
            }
            return parsedFiles;
        }

        private static void ParseFile(string yangContent)
        {

            switch (yangContent)
            {
                case string content when content.StartsWith("module"):
                    Module module = new Module();
                    module.Name = content.Split(' ')[1];
                    ParseModule(content.GetTextFromNextBrackets(0), module);
                    YangRepository._dataBase.Modules.Add(module);

                    break;
                case string content when content.StartsWith("submodule"):
                    Submodule submodule = new Submodule();
                    submodule.Name = content.Split(' ')[1];
                    ParseModule(content.GetTextFromNextBrackets(0), submodule);
                    YangRepository._dataBase.Modules.Add(submodule);
                    break;
            }
        }

        private static void ParseModule(string content, Module module)
        { 
            int index = 0;
            int lineIndex = 0;
            string[] contentLines = content.Split("\n");

            for (int i = 0; i < contentLines.Length; i++)
            {
                lineIndex = i;
                index = content.IndexOf(contentLines[i]);
                switch (contentLines[i])
                {
                    case string line when line.StartsWith("yang-version"):
                        module.YangVersion = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("namespace"):
                        module.Namespace = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("prefix"):
                        module.Prefix = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("description"):
                        module.Description = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    default:
                        continue;
                }
            }

        }

            
    }
}

