using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.InMemoryDB
{
    public static class YangRepository
    {
        public static InMemoryDB _dataBase = new InMemoryDB();
    }
}
