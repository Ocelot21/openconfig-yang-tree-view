using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Container
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Config { get; set; }
        public List<Use> Uses { get; set; }
        public List<List> Lists { get; set; }
        public List<Leaf> Leafs { get; set; }
        public List<LeafList> LeafLists { get; set; }
        public List<Container> Containers { get; set; }

        public Container()
        {
            Uses = new List<Use>();
            Lists = new List<List>();
            Leafs = new List<Leaf>();
            LeafLists = new List<LeafList>();
            Containers = new List<Container>();
        }
    }
}
