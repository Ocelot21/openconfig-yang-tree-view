using openconfig_yang_tree_view.InMemoryDB;
using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace openconfig_yang_tree_view.Services
{
    public static class ParsingService
    {
        private static string _folderPath = string.Empty;
        private static List<string> _files = new List<string>();
        private static List<string> _parsedFiles = new List<string>();
        private static List<string> _missingFiles= new List<string>();
        
        public static (List<string>, List<string>) ParseFromFolder(string path)
        {
            _folderPath = path;
            _files = Directory.GetFiles(path).ToList();
            foreach (string file in _files)
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
                        _parsedFiles.Add(fileName);
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
            foreach (Module module in YangRepository._dataBase.Modules)
            {
                foreach(string importFile in module.Imports)
                {
                    if (!ImportFileExist(importFile))
                        _missingFiles.Add(importFile);
                }
            }
            return (_parsedFiles, _missingFiles.Distinct().ToList());
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
            string[] contentLines = content.Split(Environment.NewLine, StringSplitOptions.None);

            for (int i = 0; i < contentLines.Length; i++)
            {
                int lineIndex = i;
                int index = content.IndexOf(contentLines[i]);
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
                    case string line when line.StartsWith("import"):
                        string fileName = contentLines[i].Split(" ")[1];
                        module.Imports.Add(fileName);
                        break;
                    default:
                        continue;
                }
            }

        }

        private static bool ImportFileExist(string fileName)
        {
            fileName = string.Concat(fileName, ".yang");
            if (_files.Any(f => Path.GetFileName(f) == fileName))
                return true;
            return false;
        }
    }
}

