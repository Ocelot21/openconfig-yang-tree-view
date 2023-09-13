using Microsoft.VisualBasic.ApplicationServices;
using openconfig_yang_tree_view.DataAccess;
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
        private InMemoryDb _dataBase = DataAccessService.YangDatabase;
        public void MergeModulesWithSubmodules()
        {
            foreach (var submodule in _dataBase.Modules.Where(m => m.IsSubmodule))
            {
                Module parentModule = _dataBase.Modules.SingleOrDefault(m => m.Prefix == submodule.Prefix && !m.IsSubmodule);
                if (parentModule != null)
                    parentModule.Groupings.AddRange(submodule.Groupings);
            }
            _dataBase.Modules.RemoveAll(m => m.IsSubmodule);
            Console.WriteLine(_dataBase.Modules.ToString());
        }

        public void ImplementUses()
        {

        }
    }
}
