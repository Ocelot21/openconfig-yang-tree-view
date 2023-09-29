using openconfig_yang_tree_view.DataAccess;
using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace openconfig_yang_tree_view.Services
{
    public class ModuleService
    {
        private InMemoryDb _dataBase = DataAccessService.YangDatabase;
        public void MergeModulesWithSubmodules()
        {
            foreach (var submodule in _dataBase.Modules.Where(m => m.IsSubmodule))
            {
                Module parentModule = _dataBase.Modules.FirstOrDefault(m => m.Prefix == submodule.Prefix && !m.IsSubmodule);
                if (parentModule != null)
                    parentModule.Groupings.AddRange(submodule.Groupings);
            }
            _dataBase.Modules.RemoveAll(m => m.IsSubmodule);
            Console.WriteLine(_dataBase.Modules.ToString());
        }


        public void AugmentNodes()
        {
            foreach (var module in _dataBase.Modules)
            {
                foreach (var augment in module.Augments)
                {
                    string targetModulePrefix = augment.Path[0].Split(':')[0];

                    Module targetModule = _dataBase.Modules.FirstOrDefault(m => m.Prefix == targetModulePrefix);

                    if (targetModule == null)
                    {
                        continue;
                    }

                    if (!module.Imports.Contains(targetModule.Name))
                    {
                        continue;
                    }

                    if (augment.Path.Count == 0)
                    {
                        continue;
                    }

                    if (targetModule.RootUse == null)
                    {
                        continue;
                    }

                    Grouping grouping = targetModule.Groupings.FirstOrDefault(g => g.Name == targetModule.RootUse.Name);
                    if (grouping == null)
                    {
                        continue;
                    }

                    AugmentGrouping(grouping, augment);
                }
            }
        }

        private void AugmentGrouping(Grouping grouping, Augment augment)
        {
            List<string> path = augment.Path;
            if (path.Count == 0)
            {
                return;
            }

            string containerPrefix = path[0].Split(':')[1];
            Container targetContainer = grouping.Containers.FirstOrDefault(c => c.Name == containerPrefix);
            if (targetContainer != null)
            {
                if (path.Count > 1)
                {
                    AugmentContainer(targetContainer, augment, path.Skip(1).ToList());
                }
                else
                {
                    targetContainer.Uses.AddRange(augment.Uses);
                    targetContainer.Leafs.AddRange(augment.Leafs);
                    targetContainer.Containers.AddRange(augment.Containers);
                }
            }
        }

        private void AugmentContainer(Container container, Augment augment, List<string> path)
        {
            if (path.Count == 0)
            {
                container.Uses.AddRange(augment.Uses);
                container.Leafs.AddRange(augment.Leafs);
                container.Containers.AddRange(augment.Containers);
                return;
            }
            string nodePrefix = path[0].Split(":")[1];
            Container targetContainer = container.Containers.FirstOrDefault(c => c.Name == nodePrefix);
            if (targetContainer != null)
            {
                if (path.Count > 1)
                {
                    AugmentContainer(targetContainer, augment, path.Skip(1).ToList());
                    return;
                }
                else
                {
                    targetContainer.Uses.AddRange(augment.Uses);
                    targetContainer.Leafs.AddRange(augment.Leafs);
                    targetContainer.Containers.AddRange(augment.Containers);
                    return;
                }
            }
            YangList targetList = container.Lists.FirstOrDefault(l => l.Name == nodePrefix);
            if (targetList != null)
            {
                if (path.Count > 1)
                {
                    AugmentList(targetList, augment, path.Skip(1).ToList());
                }
                else
                {
                    targetList.Uses.AddRange(augment.Uses);
                    targetList.Leafs.AddRange(augment.Leafs);
                    targetList.Containers.AddRange(augment.Containers);
                }
            }
        }

        private void AugmentList(YangList list, Augment augment, List<string> path)
        {
            if (path.Count == 0)
            {
                list.Uses.AddRange(augment.Uses);
                list.Leafs.AddRange(augment.Leafs);
                list.Containers.AddRange(augment.Containers);
                return;
            }
            string nodePrefix = path[0].Split(":")[1];
            Container targetContainer = list.Containers.FirstOrDefault(c => c.Name == nodePrefix);
            if (targetContainer != null)
            {
                if (path.Count > 1)
                {
                    AugmentContainer(targetContainer, augment, path.Skip(1).ToList());
                    return;
                }
                else
                {
                    targetContainer.Uses.AddRange(augment.Uses);
                    targetContainer.Leafs.AddRange(augment.Leafs);
                    targetContainer.Containers.AddRange(augment.Containers);
                    return;
                }
            }
            YangList targetList = list.Lists.FirstOrDefault(l => l.Name == nodePrefix);
            if (targetList != null)
            {
                if (path.Count > 1)
                {
                    AugmentList(targetList, augment, path.Skip(1).ToList());
                }
                else
                {
                    targetList.Uses.AddRange(augment.Uses);
                    targetList.Leafs.AddRange(augment.Leafs);
                    targetList.Containers.AddRange(augment.Containers);
                }
            }
        }
    }
}
