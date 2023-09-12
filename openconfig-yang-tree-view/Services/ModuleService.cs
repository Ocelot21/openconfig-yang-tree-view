using openconfig_yang_tree_view.InMemoryDB;
using openconfig_yang_tree_view.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.Services
{
    public class ModuleService
    {
        //public void MergeModulesWithSubmodules()
        //{
        //    List<Module> submodules = new List<Module>();
        //    foreach(var module in DataAccessService._dataBase.Modules) 
        //    {
        //        if (module is Submodule)
        //        {
        //            var parentModule = DataAccessService._dataBase.Modules.SingleOrDefault(m => m.Name == (module as Submodule).ParentPrefix);
        //            if (parentModule != null)
        //            {
        //                parentModule.Groupings.AddRange(module.Groupings);
        //                submodules.Add(module);
        //            }
                    
        //        }
        //    }
        //    foreach (var submodule in submodules)
        //    {
        //        DataAccessService._dataBase.Modules.Remove(submodule);
        //    }
        //}

    }
}
