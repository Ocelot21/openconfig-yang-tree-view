using Microsoft.VisualBasic.ApplicationServices;
using openconfig_yang_tree_view.DataAccess;
using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Shapes;
using static System.Windows.Forms.LinkLabel;

namespace openconfig_yang_tree_view.Services
{
    public class ParsingService
    {
        private string _folderPath = string.Empty;
        private List<string> _files = new List<string>();
        private List<string> _parsedFiles = new List<string>();
        private List<string> _missingFiles = new List<string>();

        private InMemoryDb _dataBase = DataAccessService.YangDatabase;
        private ModuleService _moduleService = new ModuleService();

        public (List<string>, List<string>) ParseFromFolder(string path)
        {
            _folderPath = path;
            _files = Directory.GetFiles(path).ToList();
            foreach (string file in _files)
            {
                if (file.EndsWith(".yang"))
                {
                    try
                    {
                        List<string> yangFileLines = File.ReadAllLines(file).ToList();
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                        //Console.WriteLine($"Parsing {fileName}.yang");
                        yangFileLines = yangFileLines.TrimAllLines().ToList();
                        ParseFile(yangFileLines);
                        //Console.WriteLine($"End of {fileName}.yang\n");
                        _parsedFiles.Add(fileName);
                        Console.WriteLine(DataAccessService.YangDatabase.Modules.Count);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message} \n {ex.InnerException}");
                    }
                }
                else
                    continue;
            }

            var submodules = DataAccessService.YangDatabase.Modules.Where(m => m.IsSubmodule);

            _moduleService.MergeModulesWithSubmodules();


            foreach (Module module in _dataBase.Modules)
            {
                foreach (string importFile in module.Imports)
                {
                    if (!ImportFileExist(importFile))
                        _missingFiles.Add(importFile);
                }
                module.Imports.RemoveAll(mp => _missingFiles.Contains(mp));
            }


            return (_parsedFiles, _missingFiles.Distinct().ToList());
        }

        private void ParseFile(List<string> yangFileLines)
        {
            switch (yangFileLines[0])
            {
                case string line when line.StartsWith("module"):
                    Module module = new Module();
                    module.Name = line.Split(' ')[1];
                    module.IsSubmodule = false;
                    ParseModule(yangFileLines.ToList(), module);
                    _dataBase.Modules.Add(module);
                    break;
                case string line when line.StartsWith("submodule"):
                    Module submodule = new Module();
                    submodule.Name = line.Split(' ')[1];
                    submodule.IsSubmodule = true;
                    ParseModule(yangFileLines.Skip(1).ToList(), submodule);
                    _dataBase.Modules.Add(submodule);
                    break;
            }
        }

        private void ParseModule(List<string> moduleLines, Module module)
        {
            module.YangVersion = "1";
            List<List<string>> groupingsLines = new List<List<string>>();

            for (int i = 0; i < moduleLines.Count; i++)
            {
                if (moduleLines[i].StartsWith("//"))
                {
                    continue;
                }

                switch (moduleLines[i])
                {
                    case string line when line.StartsWith("belongs-to"):
                        List<string> belongsTo = moduleLines.GetObjectContent(i, out int belongsToIndex).ToList();
                        foreach (var belongsToLine in belongsTo)
                        {
                            if (belongsToLine.StartsWith("prefix"))
                            {
                                module.Prefix = belongsToLine.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", string.Empty).Replace(";", string.Empty);
                            }                               
                        }
                        i = belongsToIndex;
                        break;
                    case string line when line.StartsWith("yang-version"):
                        module.YangVersion = line.Split(' ')[1].Replace(";", string.Empty).Replace("\"", string.Empty);
                        break;
                    case string line when line.StartsWith("namespace"):
                        module.Namespace = line.Split(' ')[1].Replace(";", string.Empty).Replace("\"", string.Empty);
                        break;
                    case string line when line.StartsWith("prefix"):
                        string prefix = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1].Replace(";", string.Empty).Replace("\"", string.Empty);
                        module.Prefix = prefix;
                        break;
                    case string line when line.StartsWith("import"):
                        string import = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1].Replace(";", string.Empty).Replace("\"", string.Empty);
                        module.Imports.Add(import);
                        break;
                    case string line when line.StartsWith("include"):
                        module.Includes.Add(line.Split(" ")[1].Replace("\"", string.Empty).Replace(";", String.Empty));
                        break;
                    case string line when line.StartsWith("description"):
                        module.Description = moduleLines.GetMultilineText(i, out int descriptionIndex);
                        i = descriptionIndex;
                        break;
                    case string line when line.StartsWith("uses"):
                        Use use = new Use();
                        use.Name = line.Split(' ')[1].Replace(";", string.Empty);
                        if (use.Name.Contains(":"))
                        {
                            use.Prefix = use.Name.Split(':')[0];
                            use.Name = use.Name.Split(':')[1];
                        }
                        else
                        {
                            use.Prefix = module.Prefix;
                        }
                        module.RootUse = use;
                        break;
                    case string line when line.StartsWith("identity"):
                        moduleLines.GetObjectContent(i, out int identityIndex);
                        i = identityIndex;
                        break;
                    case string line when line.StartsWith("revision"):
                        moduleLines.GetObjectContent(i, out int revisionIndex);
                        i = revisionIndex;
                        break;
                    case string line when line.StartsWith("leaf "):
                        Leaf leaf = new Leaf();
                        leaf.Name = line.Split(' ')[1];
                        List<string> leafLines = moduleLines.GetObjectContent(i, out int leafIndex).ToList();
                        ParseLeaf(leafLines, leaf);
                        module.Leafs.Add(leaf);
                        i = leafIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = line.Split(' ')[1];
                        List<string> leafListLines = moduleLines.GetObjectContent(i, out int leafListIndex).ToList();
                        ParseLeaf(leafListLines, leafList);
                        module.Leafs.Add(leafList);
                        i = leafListIndex;
                        break;
                    case string line when line.StartsWith("container"):
                        Container container = new Container();
                        container.Name = line.Split(' ')[1];
                        List<string> containerLines = moduleLines.GetObjectContent(i, out int containerIndex).ToList();
                        ParseContainer(containerLines, container, module.Prefix);
                        if (!module.Containers.Contains(container))
                        {
                            module.Containers.Add(container);
                        }
                        i = containerIndex;
                        break;
                    case string line when line.StartsWith("grouping"):
                        List<string> groupingLines = moduleLines.GetObjectContent(i, out int groupingIndex).ToList();
                        groupingsLines.Add(groupingLines);
                        i = groupingIndex;

                        break;
                    case string line when line.StartsWith("augment"):
                        Augment augment = new Augment();
                        augment.Name = line.Split(' ')[1];
                        List<string> augmentLines = moduleLines.GetObjectContent(i, out int augmentIndex).ToList();
                        ParseAugment(augmentLines, augment, module.Prefix);
                        module.Augments.Add(augment);
                        i = augmentIndex;
                        break;
                }
            }

            module.Groupings = GetAllGroupings(groupingsLines, module.Prefix);

            if (module.RootUse != null)
            {
                module.Root = module.Groupings.FirstOrDefault(x => x.Name == module.RootUse.Name);
            }
        }

        private List<Grouping> GetAllGroupings(List<List<string>> groupingsLines, string modulePrefix)
        {
            List<Grouping> groupings = new List<Grouping>();

            foreach (var groupingLines in groupingsLines)
            {
                Grouping grouping = new Grouping();
                grouping.Name = groupingLines[0].Split(' ')[1];
                for (int i = 0; i < groupingLines.Count; i++)
                {
                    switch (groupingLines[i])
                    {
                        case string line when line.StartsWith("description"):
                            grouping.Description = groupingLines.GetMultilineText(i, out int descriptionIndex);
                            i = descriptionIndex;
                            break;
                        case string line when line.StartsWith("container"):
                            Container container = new Container();
                            container.Name = line.Split(' ')[1];
                            List<string> containerLines = groupingLines.GetObjectContent(i, out int containerIndex).ToList();
                            ParseContainer(containerLines, container, modulePrefix);
                            grouping.Containers.Add(container);
                            i = containerIndex;
                            break;
                        case string line when line.StartsWith("list"):
                            YangList list = new YangList();
                            list.Name = line.Split(' ')[1];
                            List<string> listLines = groupingLines.GetObjectContent(i, out int listIndex).ToList();
                            ParseList(listLines, list, modulePrefix);
                            grouping.Lists.Add(list);
                            i = listIndex;
                            break;
                        case string line when line.StartsWith("leaf "):
                            Leaf leaf = new Leaf();
                            leaf.Name = line.Split(' ')[1];
                            List<string> leafLines = groupingLines.GetObjectContent(i, out int leafIndex).ToList();
                            ParseLeaf(leafLines, leaf);
                            grouping.Leafs.Add(leaf);
                            i = leafIndex;
                            break;
                        case string line when line.StartsWith("leaf-list"):
                            LeafList leafList = new LeafList();
                            leafList.Name = line.Split(' ')[1];
                            List<string> leafListLines = groupingLines.GetObjectContent(i, out int leafListIndex).ToList();
                            ParseLeaf(leafListLines, leafList);
                            grouping.Leafs.Add(leafList);
                            i = leafListIndex;
                            break;
                        case string line when line.StartsWith("uses"):
                            Use use = new Use();
                            use.Name = line.Split(' ')[1].Replace(";", string.Empty);
                            if (use.Name.Contains(":"))
                            {
                                use.Prefix = use.Name.Split(':')[0];
                                use.Name = use.Name.Split(':')[1];
                            }
                            else
                            {
                                use.Prefix = modulePrefix;
                            }
                            grouping.Uses.Add(use);
                            break;
                    }
                }
                groupings.Add(grouping);
            }
            return groupings;
        }

        private void ParseList(List<string> listLines, YangList list, string modulePrefix)
        {
            for (int i = 0; i < listLines.Count; i++)
            {
                switch (listLines[i])
                {
                    case string line when line.StartsWith("description"):
                        list.Description = listLines.GetMultilineText(i, out int descriptionIndex);
                        i = descriptionIndex;
                        break;
                    case string line when line.StartsWith("key"):
                        int indexOfFirst = line.IndexOf('"');
                        int indexOfLast = line.LastIndexOf('"');
                        list.Key = line.Substring(indexOfFirst, indexOfLast - indexOfFirst).Trim();
                        break;
                    case string line when line.StartsWith("leaf "):
                        Leaf leaf = new Leaf();
                        leaf.Name = line.Split(' ')[1];
                        List<string> leafLines = listLines.GetObjectContent(i, out int leafIndex).ToList();
                        ParseLeaf(leafLines, leaf);
                        list.Leafs.Add(leaf);
                        i = leafIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = line.Split(' ')[1];
                        List<string> leafListLines = listLines.GetObjectContent(i, out int leafListIndex).ToList();
                        ParseLeaf(leafListLines, leafList);
                        list.Leafs.Add(leafList);
                        i = leafListIndex;
                        break;
                    case string line when line.StartsWith("container"):
                        Container container = new Container();
                        container.Name = line.Split(" ")[1];
                        List<string> containerLines = listLines.GetObjectContent(i, out int containerIndex).ToList();
                        ParseContainer(containerLines, container, modulePrefix);
                        list.Containers.Add(container);
                        i = containerIndex;
                        break;
                    case string line when line.StartsWith("uses"):
                        Use use = new Use();
                        use.Name = line.Split(' ')[1].Replace(";", string.Empty);
                        if (use.Name.Contains(":"))
                        {
                            use.Prefix = use.Name.Split(':')[0];
                            use.Name = use.Name.Split(':')[1];
                        }
                        else
                        {
                            use.Prefix = modulePrefix;
                        }
                        list.Uses.Add(use);
                        break;
                    case string line when line.StartsWith("list") && i != 0:
                        YangList sublist = new YangList();
                        sublist.Name = line.Split(' ')[1];
                        List<string> sublistLines = listLines.GetObjectContent(i, out int listIndex).ToList();
                        ParseList(sublistLines, sublist, modulePrefix);
                        list.Lists.Add(sublist);
                        i = listIndex;
                        break;
                }
            }
        }

        private void ParseAugment(List<string> augmentLines, Augment augment, string modulePrefix)
        {
            for (int i = 0;  i < augmentLines.Count; i++)
            {
                switch(augmentLines[i])
                {
                    case string line when line.StartsWith("description"):
                        augment.Description = augmentLines.GetMultilineText(i, out int descriptionIndex);
                        i = descriptionIndex;
                        break;
                    case string line when line.StartsWith("uses"):
                        Use use = new Use();
                        use.Name = line.Split(' ')[1].Replace(";", string.Empty);
                        if (use.Name.Contains(":"))
                        {
                            use.Prefix = use.Name.Split(':')[0];
                            use.Name = use.Name.Split(':')[1];
                        }
                        else
                        {
                            use.Prefix = modulePrefix;
                        }
                        augment.Uses.Add(use);
                        break;
                    case string line when line.StartsWith("leaf "):
                        Leaf leaf = new Leaf();
                        leaf.Name = line.Split(' ')[1];
                        var leafLines = augmentLines.GetObjectContent(i, out int leafIndex).ToList();
                        ParseLeaf(leafLines, leaf);
                        augment.Leafs.Add(leaf);
                        i = leafIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = line.Split(' ')[1];
                        var leafListLines = augmentLines.GetObjectContent(i, out int leafListIndex).ToList();
                        ParseLeaf(leafListLines, leafList);
                        augment.Leafs.Add(leafList);
                        i = leafListIndex;
                        break;

                }
            }
        }

        private void ParseContainer(List<string> containerLines, Container container, string modulePrefix)
        {
            container.Config = true; // default
            for (int i = 0; i < containerLines.Count; i++)
            {
                switch (containerLines[i])
                {
                    case string line when line.StartsWith("description"):
                        container.Description = containerLines.GetMultilineText(i, out int descriptionIndex);
                        i = descriptionIndex;
                        break;
                    case string line when line.StartsWith("uses"):
                        Use use = new Use();
                        use.Name = line.Split(' ')[1].Replace(";", string.Empty);
                        if (use.Name.Contains(":"))
                        {
                            use.Prefix = use.Name.Split(':')[0];
                            use.Name = use.Name.Split(':')[1];
                        }
                        else
                        {
                            use.Prefix = modulePrefix;
                        }
                        container.Uses.Add(use);
                        break;
                    case string line when line.StartsWith("leaf "):
                        Leaf leaf = new Leaf();
                        leaf.Name = line.Split(' ')[1];
                        var leafLines = containerLines.GetObjectContent(i, out int leafIndex).ToList();
                        ParseLeaf(leafLines, leaf);
                        container.Leafs.Add(leaf);
                        i = leafIndex;
                        break;
                    case string line when line.StartsWith("leaf-list"):
                        LeafList leafList = new LeafList();
                        leafList.Name = line.Split(' ')[1];
                        var leafListLines = containerLines.GetObjectContent(i, out int leafListIndex).ToList();
                        ParseLeaf(leafListLines, leafList);
                        container.Leafs.Add(leafList);
                        i = leafListIndex;
                        break;
                    case string line when line.StartsWith("container") && i != 0:
                        Container subcontainer = new Container();
                        subcontainer.Name = line.Split(' ')[1];
                        var subcontainerLines = containerLines.GetObjectContent(i, out int subcontainerIndex).ToList();
                        ParseContainer(subcontainerLines, subcontainer, modulePrefix);
                        if (!container.Containers.Contains(subcontainer))
                        {
                            container.Containers.Add(subcontainer);
                        }
                        i = subcontainerIndex;
                        break;
                    case string line when line.StartsWith("list"):
                        YangList list = new YangList();
                        list.Name = line.Split(' ')[1];
                        List<string> listLines = containerLines.GetObjectContent(i, out int listIndex).ToList();
                        ParseList(listLines, list, modulePrefix);
                        container.Lists.Add(list);
                        i = listIndex;
                        break;
                }
            }
        }

        private void ParseLeaf(List<string> leafLines, Leaf leaf)
        {
            leaf.Config = true; //Default for leaf
            for (int i = 0; i < leafLines.Count; i++)
            {
                switch (leafLines[i])
                {
                    case string line when line.StartsWith("description"):
                        leaf.Description = leafLines.GetMultilineText(i, out int descriptionIndex);
                        i = descriptionIndex;
                        break;
                    case string line when line.StartsWith("config false"):
                        leaf.Config = false;
                        break;
                    case string line when line.StartsWith("type"):
                        leaf.Type = leafLines[i].Split(' ')[1].Split(';')[0];
                        break;
                    default:
                        continue;
                }
            }
        }

        private void ImplementUsesToAllNodes()
        {

        }

        private bool ImportFileExist(string fileName)
        {
            fileName = string.Concat(fileName, ".yang");
            if (_files.Any(f => System.IO.Path.GetFileName(f) == fileName))
                return true;
            return false;
        }
    }
}