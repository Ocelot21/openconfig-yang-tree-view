using System;
using System.Collections.Generic;

namespace openconfig_yang_tree_view.MVVM.Model
{
    public class Module
    {
        public string Name { get; set; }

        public string Prefix { get; set; }

        public string YangVersion { get; set; }

        public string Namespace { get; set; }

        public string Description { get; set; }

        //public string Organization { get; set; }

        //public string Contact { get; set; }

        public bool IsSubmodule { get; set; }

        public List<string> Imports { get; set; }

        public List<string> Includes { get; set; }

        public List<Grouping> Groupings { get; set; }

        public Use RootUse { get; set; }

        public List<Use> Uses { get; set; }

        public  List<Augment> Augments { get; set; }

        public Module()
        {
            YangVersion = "1";
            Imports = new List<string>();
            Includes = new List<string>();
            Groupings = new List<Grouping>();
            Uses = new List<Use>();
            Augments = new List<Augment>();
        }
    }
}
