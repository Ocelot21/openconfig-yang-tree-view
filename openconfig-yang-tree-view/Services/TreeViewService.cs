using Mapster;
using Microsoft.VisualBasic.ApplicationServices;
using openconfig_yang_tree_view.DataAccess;
using openconfig_yang_tree_view.MVVM.Model;
using openconfig_yang_tree_view.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private GroupingViewModel CreateGroupingViewModel(Grouping root)
        {
            return null;
        }

        //public void FillTree(Tree tree)
        //{

        //    foreach (var module in _dataBase.Modules.Where(m => m.Root != null))
        //    {
        //        foreach (var container in module.Root.Containers)
        //        {
        //            ImplementUsesToContainer(container);
        //        }

        //        tree.Roots.Add(module.Root);
        //    }

        //    if (tree.Roots.Count == 0)
        //    {
        //        foreach (var module in _dataBase.Modules)
        //        {
        //            module.AddUsesFromAllNodes();
        //            foreach (var grouping in module.Groupings)
        //            {
        //                if (!module.Uses.Any(u => u.Name == grouping.Name))
        //                {
        //                    foreach (var container in grouping.Containers)
        //                    {
        //                        ImplementExternalUsesToContainer(container);
        //                    }

        //                    tree.Roots.Add(grouping);
        //                }
        //            }
        //        }
        //    }
        //}

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
                container.Containers.Add(subContainer);
                ImplementUsesToContainer(subContainer);
            }

            foreach (var subUses in grouping.Uses)
            {
                ImplementUse(container, subUses);
            }
        }

        //private void ImplementInternalUses()
    }
}
