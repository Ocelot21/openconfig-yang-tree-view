using Mapster;
using openconfig_yang_tree_view.DataAccess;
using openconfig_yang_tree_view.MVVM.Model;
using openconfig_yang_tree_view.MVVM.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace openconfig_yang_tree_view.Services
{
    public class TreeViewService
    {
        private InMemoryDb _dataBase = DataAccessService.YangDatabase;
        private ModuleService _moduleService;

        public TreeViewService() 
        {
            _moduleService = new ModuleService();
        }

        public void FillTree(TreeViewModel treeViewModel)
        {
            _moduleService.AugmentNodes();
            foreach (var module in _dataBase.Modules.Where(m => m.RootUse != null))
            {
                Grouping root = module.Groupings.FirstOrDefault(g => g.Name == module.RootUse.Name);

                if (root != null)
                {
                    foreach (var container in root.Containers)
                    {
                        ImplementUsesToContainer(container);
                    }

                    var groupingViewModel = root.Adapt<GroupingViewModel>();

                    treeViewModel.Roots.Add(groupingViewModel);
                }
            }

            GeneratePaths(treeViewModel.Roots);
        }


        private void ImplementUsesToContainer(Container container)
        {
            foreach (var subContainer in container.Containers)
            {
                ImplementUsesToContainer(subContainer);
            }

            foreach (var use in container.Uses)
            {
                if (!use.IsImplemented)
                {
                    ImplementUse(container, use);
                    use.IsImplemented = true;
                }
            }

            foreach (var list in container.Lists)
            {
                ImplementUsesToList(list);

                foreach (var subContainer in list.Containers)
                {
                    ImplementUsesToContainer(subContainer);
                }
            }
        }

        private void ImplementUse(Container container, Use use)
        {
            Module module = _dataBase.Modules.FirstOrDefault(m => m.Prefix == use.Prefix);
            if (module == null)
            {
                return;
            }

            Grouping grouping = module.Groupings.FirstOrDefault(m => m.Name == use.Name);
            if (grouping == null)
            {
                return;
            }

            foreach (var leaf in grouping.Leafs)
            {
                if (!container.Leafs.Contains(leaf))
                {
                    container.Leafs.Add(leaf);
                }
            }

            foreach (var subContainer in grouping.Containers)
            {
                ImplementUsesToContainer(subContainer);
                container.Containers.Add(subContainer);
            }

            foreach (var subUse in grouping.Uses)
            {
                ImplementUse(container, subUse);
            }
        }

        private void ImplementUsesToList(YangList list)
        {
            foreach (var use in list.Uses)
            {
                if (use.IsImplemented)
                {
                    continue;
                }

                Module module = _dataBase.Modules.FirstOrDefault(m => m.Prefix == use.Prefix);
                if (module == null)
                {
                    continue;
                }

                Grouping grouping = module.Groupings.FirstOrDefault(g =>  g.Name == use.Name);
                if (grouping == null)
                {
                    continue;
                }

                foreach (var container in grouping.Containers)
                {
                    if (!list.Containers.Contains(container))
                    {
                        ImplementUsesToContainer(container);
                        list.Containers.Add(container);                    
                    }
                }

                foreach (var leaf in grouping.Leafs)
                {
                    if (!list.Leafs.Contains(leaf))
                    {
                        list.Leafs.Add(leaf);
                    }
                }

            }
        }

        public ModuleViewModel GetModuleViewModelByPrefix(string prefix)
        {
            Module module = null;

            module = _dataBase.Modules.FirstOrDefault(m => m.Prefix == prefix);

            if (module == null)
            {
                return null;
            }

            ModuleViewModel moduleViewModel = new ModuleViewModel
            {
                Name = module.Name,
                Prefix = module.Prefix,
                Namespace = module.Namespace,
                YangVersion = module.YangVersion,
                Description = module.Description
            };

            return moduleViewModel;
        }

        private void GeneratePaths(ObservableCollection<GroupingViewModel> roots)
        {
            foreach (var grouping in roots)
            {
                GeneratePaths(grouping, string.Empty);
            }
        }

        private void GeneratePaths(TreeNodeViewModel node, string currentPath)
        {
            if (node is GroupingViewModel)
            {
                foreach (var subNode in node.Children)
                {
                    GeneratePaths(subNode, currentPath);
                }
            }
            else if (node is LeafViewModel || node is ListViewModel || node is ContainerViewModel)
            {
                currentPath += "/" + node.Name;
                ((dynamic)node).Path = currentPath;

                foreach (var subNode in node.Children)
                {
                    GeneratePaths(subNode, currentPath);
                }
            }
        }
    }
}
