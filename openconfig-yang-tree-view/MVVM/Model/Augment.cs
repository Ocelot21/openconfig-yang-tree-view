using System.Collections.Generic;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Augment
    {
        public List<string> Path { get; set; }
        public string Description { get; set; }
        public List<Container> Containers { get; set; }
        public List<string> Uses { get; set; }
        public List<Leaf> Leafs { get; set; }

        public Augment()
        {
            Uses = new List<string>();
            Leafs = new List<Leaf>();
        }
    }
}
