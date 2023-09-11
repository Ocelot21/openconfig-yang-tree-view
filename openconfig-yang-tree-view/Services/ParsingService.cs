using openconfig_yang_tree_view.InMemoryDB;
using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace openconfig_yang_tree_view.Services
{
    public static class ParsingService
    {
        private static string _folderPath = string.Empty;
        private static List<string> _files = new List<string>();
        private static List<string> _parsedFiles = new List<string>();
        private static List<string> _missingFiles = new List<string>();

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
                        Console.WriteLine(DataAccessService._dataBase.Modules.Count);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} \n {ex.InnerException}");
                    }
                }
                else
                    continue;
            }
            foreach (Module module in DataAccessService._dataBase.Modules)
            {
                foreach (string importFile in module.Imports)
                {
                    if (!ImportFileExist(importFile))
                        _missingFiles.Add(importFile);
                }
            }
            return (_parsedFiles, _missingFiles.Distinct().ToList());
        }

        private static void ParseFile(string yangContent)
        {
            int lineIndex = 0;

            switch (yangContent)
            {
                case string content when content.StartsWith("module"):
                    Module module = new Module();
                    module.Name = content.Split(' ')[1];
                    ParseModule(content.GetTextFromNextBrackets(0, ref lineIndex), module);
                    DataAccessService._dataBase.Modules.Add(module);

                    break;
                case string content when content.StartsWith("submodule"):
                    Submodule submodule = new Submodule();
                    submodule.Name = content.Split(' ')[1];
                    ParseModule(content.GetTextFromNextBrackets(0, ref lineIndex), submodule);
                    DataAccessService._dataBase.Modules.Add(submodule);
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
                    case string line when line.StartsWith("belongs-to"):
                        if (module is Submodule)
                            (module as Submodule).ParentPrefix = contentLines[i].Split(' ')[1];
                        break;
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
                    case string line when line.StartsWith("revision"):
                        content.GetTextFromNextBrackets(index, ref lineIndex);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("container"):
                        Container container = new Container();
                        container.Name = contentLines[i].Split(" ")[1];
                        container.Config = true;
                        ParseContainer(container, content.GetTextFromNextBrackets(index, ref lineIndex));
                        module.Containers.Add(container);
                        i = lineIndex;
                        break;
                    case string line when (line.StartsWith("leaf") && !line.StartsWith("leaf-list")):
                        Leaf leaf = new Leaf();
                        leaf.Name = contentLines[i].Split(" ")[1];
                        leaf.Config = true;
                        ParseLeaf(leaf, content.GetTextFromNextBrackets(index, ref lineIndex));
                        module.Leafs.Add(leaf);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = contentLines[i].Split(" ")[1];
                        leafList.Config = true;
                        ParseLeaf(leafList, content.GetTextFromNextBrackets(index, ref lineIndex));
                        module.LeafLists.Add(leafList);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("grouping"):
                        Grouping grouping = new Grouping();
                        grouping.Name = contentLines[i].Split(" ")[1];
                        ParseGrouping(grouping, content.GetTextFromNextBrackets(index, ref lineIndex));
                        module.Groupings.Add(grouping);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("list"):
                        YangList list = new YangList();
                        list.Name = contentLines[i].Split(" ")[1];
                        ParseList(list, content.GetTextFromNextBrackets(index, ref lineIndex));
                        module.Lists.Add(list);
                        i = lineIndex;
                        break;
                    default:
                        continue;
                }
            }

        }

        private static void ParseList(YangList list, string content)
        {
            string[] contentLines = content.Split(Environment.NewLine, StringSplitOptions.None);

            for (int i = 0; i < contentLines.Length; i++)
            {
                int lineIndex = i;
                int index = content.IndexOf(contentLines[i]);
                switch (contentLines[i])
                {
                    case string line when line.StartsWith("description"):
                        list.Description = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("container"):
                        Container container = new Container();
                        container.Name = contentLines[i].Split(" ")[1];
                        container.Config = true;
                        ParseContainer(container, content.GetTextFromNextBrackets(index, ref lineIndex));
                        list.Containers.Add(container);
                        i = lineIndex;
                        break;
                    case string line when (line.StartsWith("leaf") && !line.StartsWith("leaf-list")):
                        Leaf leaf = new Leaf();
                        leaf.Name = contentLines[i].Split(" ")[1];
                        leaf.Config = true;
                        ParseLeaf(leaf, content.GetTextFromNextBrackets(index, ref lineIndex));
                        list.Leafs.Add(leaf);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = contentLines[i].Split(" ")[1];
                        leafList.Config = true;
                        ParseLeaf(leafList, content.GetTextFromNextBrackets(index, ref lineIndex));
                        list.LeafLists.Add(leafList);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("grouping"):
                        Grouping grouping = new Grouping();
                        grouping.Name = contentLines[i].Split(" ")[1];
                        ParseGrouping(grouping, content.GetTextFromNextBrackets(index, ref lineIndex));
                        list.Groupings.Add(grouping);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("list"):
                        YangList innerList = new YangList();
                        innerList.Name = contentLines[i].Split(" ")[1];
                        ParseList(innerList, content.GetTextFromNextBrackets(index, ref lineIndex));
                        list.Lists.Add(innerList);
                        i = lineIndex;
                        break;
                    default:
                        continue;
                }
            }
        }

        private static void ParseGrouping(Grouping grouping, string content)
        {
            string[] contentLines = content.Split(Environment.NewLine, StringSplitOptions.None);

            for (int i = 0; i < contentLines.Length; i++)
            {
                int lineIndex = i;
                int index = content.IndexOf(contentLines[i]);
                switch (contentLines[i])
                {
                    case string line when line.StartsWith("description"):
                        grouping.Description = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("container"):
                        Container container = new Container();
                        container.Name = contentLines[i].Split(" ")[1];
                        container.Config = true;
                        ParseContainer(container, content.GetTextFromNextBrackets(index, ref lineIndex));
                        grouping.Containers.Add(container);
                        i = lineIndex;
                        break;
                    case string line when (line.StartsWith("leaf") && !line.StartsWith("leaf-list")):
                        Leaf leaf = new Leaf();
                        leaf.Name = contentLines[i].Split(" ")[1];
                        leaf.Config = true;
                        ParseLeaf(leaf, content.GetTextFromNextBrackets(index, ref lineIndex));
                        grouping.Leafs.Add(leaf);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = contentLines[i].Split(" ")[1];
                        leafList.Config = true;
                        ParseLeaf(leafList, content.GetTextFromNextBrackets(index, ref lineIndex));
                        grouping.LeafLists.Add(leafList);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("grouping"):
                        Grouping innerGrouping = new Grouping();
                        innerGrouping.Name = contentLines[i].Split(" ")[1];
                        ParseGrouping(innerGrouping, content.GetTextFromNextBrackets(index, ref lineIndex));
                        grouping.Groupings.Add(innerGrouping);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("list"):
                        YangList list = new YangList();
                        list.Name = contentLines[i].Split(" ")[1];
                        ParseList(list, content.GetTextFromNextBrackets(index, ref lineIndex));
                        grouping.Lists.Add(list);
                        i = lineIndex;
                        break;
                    //Lists, Uses
                    default:
                        continue;
                }
            }
        }

        private static void ParseContainer(Container container, string content)
        {
            string[] contentLines = content.Split(Environment.NewLine, StringSplitOptions.None);

            for (int i = 0; i < contentLines.Length; i++)
            {
                int lineIndex = i;
                int index = content.IndexOf(contentLines[i]);
                switch (contentLines[i])
                {
                    case string line when line.StartsWith("description"):
                        container.Description = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("config false"):
                        container.Config = false;
                        break;
                    case string line when line.StartsWith("container"):
                        Container innerContainer = new Container();
                        innerContainer.Name = contentLines[i].Split(" ")[1];
                        innerContainer.Config = true;
                        ParseContainer(innerContainer, content.GetTextFromNextBrackets(index, ref lineIndex));
                        container.Containers.Add(innerContainer);
                        i = lineIndex;
                        break;
                    case string line when (line.StartsWith("leaf") && !line.StartsWith("leaf-list")):
                        Leaf leaf = new Leaf();
                        leaf.Name = contentLines[i].Split(" ")[1];
                        leaf.Config = true;
                        ParseLeaf(leaf, content.GetTextFromNextBrackets(index, ref lineIndex));
                        container.Leafs.Add(leaf);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = contentLines[i].Split(" ")[1];
                        leafList.Config = true;
                        ParseLeaf(leafList, content.GetTextFromNextBrackets(index, ref lineIndex));
                        container.LeafLists.Add(leafList);
                        i = lineIndex;
                        break;
                    case string line when line.StartsWith("list"):
                        YangList list = new YangList();
                        list.Name = contentLines[i].Split(" ")[1];

                        ParseList(list, content.GetTextFromNextBrackets(index, ref lineIndex));
                        container.Lists.Add(list);
                        i = lineIndex;
                        break;
                    //Lists, Uses
                    default:
                        continue;
                }
            }
        }

        private static void ParseLeaf(Leaf leaf, string content)
        {
            string[] contentLines = content.Split(Environment.NewLine, StringSplitOptions.None);

            for (int i = 0; i < contentLines.Length; i++)
            {
                int lineIndex = i;
                int index = content.IndexOf(contentLines[i]);
                switch (contentLines[i])
                {
                    case string line when line.StartsWith("description"):
                        leaf.Description = content.GetTextFromNextQuotation(index, ref lineIndex);
                        i = lineIndex - 1;
                        break;
                    case string line when line.StartsWith("config false"):
                        leaf.Config = false;
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
