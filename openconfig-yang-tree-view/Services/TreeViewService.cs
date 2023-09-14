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
        public Tree Tree { get; set; }

        public TreeViewService() 
        {
            Tree = new Tree();
            _moduleService = new ModuleService();
        }

        public void FillTree()
        {

            foreach (var module in _dataBase.Modules.Where(m => m.Root != null))
            {
                foreach (var container in module.Root.Containers)
                {
                    ImplementUsesToContainer(container);
                }

                Tree.Roots.Add(module.Root);
            }
            
            if (Tree.Roots.Count == 0)
            {
                foreach(var module in _dataBase.Modules)
                {
                    module.AddUsesFromAllNodes();
                    foreach(var grouping in module.Groupings)
                    {
                        if (!module.Uses.Any(m => m.Name == grouping.Name))
                        {
                            foreach (var container in grouping.Containers)
                            {
                                ImplementUsesToContainer(container);
                            }

                            Tree.Roots.Add(grouping);
                        }
                    }
                }
            }
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
        }

        private void ImplementUse(Container container, Use use)
        {
            var module = _dataBase.Modules.FirstOrDefault(m => m.Prefix == use.ExternalPrefix);
            
            if (module == null)
            {
                return;
            }

            var grouping = module.Groupings.FirstOrDefault(m => m.Name == use.Name);
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
    }
}
