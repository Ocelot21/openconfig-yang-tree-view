﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openconfig_yang_tree_view.MVVM.ViewModels
{
    public class TreeNodeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ObservableCollection<TreeNodeViewModel> Children { get; set; }
        public TreeNodeViewModel() 
        { 
            Children = new ObservableCollection<TreeNodeViewModel>();
        }
    }
}
