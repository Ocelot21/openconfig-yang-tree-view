using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Augment
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Use> Uses { get; set; }

        public List<string> FilePrefixes { get; set; }

        public List<string> Path { get; set; }

        public List<Leaf> Leafs { get; set; }

        public Augment()
        {
            Uses = new List<Use>();
            Leafs = new List<Leaf>();
        }
    }
}
