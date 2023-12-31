﻿using openconfig_yang_tree_view.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace openconfig_yang_tree_view.MVVM.Views
{
    /// <summary>
    /// Interaction logic for LeafPropertyGridControl.xaml
    /// </summary>
    public partial class LeafPropertyGridControl : UserControl
    {
        public LeafPropertyGridControl(LeafViewModel leafViewModel)
        {
            InitializeComponent();
            DataContext = leafViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LeafViewModel leafViewModel)
            {
                var gnmiGetWindow = new GnmiGetWindow(leafViewModel);
                gnmiGetWindow.ShowDialog();
            }         
        }
    }
}
