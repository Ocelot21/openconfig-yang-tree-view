﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class ContainerViewModel : TreeNodeViewModel
    { 
        public bool Config { get; set; }
        public string Path { get; set; }
    }
}
