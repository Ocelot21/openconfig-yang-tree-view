using openconfig_yang_tree_view.DataAccess;
using openconfig_yang_tree_view.MVVM.Model;
using openconfig_yang_tree_view.MVVM.ViewModels;
using openconfig_yang_tree_view.Services;
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

    public partial class YangTreeView : UserControl
    {
        private TreeViewService _treeViewService = new TreeViewService();
        private TreeViewModel _tree = new TreeViewModel();

        public YangTreeView()
        {
            InitializeComponent();
            _treeViewService.FillTree(_tree);
            DataContext = _tree;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedObject = treeView.SelectedItem;

            if (selectedObject is LeafViewModel)
            {
                propertyGridPlaceholder.Children.Clear();
                propertyGridPlaceholder.Children.Add(new LeafPropertyGridControl(selectedObject as LeafViewModel));
            }
            else if (selectedObject is ContainerViewModel)
            {
                propertyGridPlaceholder.Children.Clear();
                propertyGridPlaceholder.Children.Add(new ContainerPropertyGridControl(selectedObject as ContainerViewModel));
            }
            else if (selectedObject is GroupingViewModel)
            {
                propertyGridPlaceholder.Children.Clear();
                var grouping = selectedObject as GroupingViewModel;
                if (grouping != null)
                {
                    ModuleViewModel moduleViewModel = _treeViewService.GetModuleViewModelByPrefix(grouping.Prefix);
                    {
                        if (moduleViewModel != null)
                        {
                            propertyGridPlaceholder.Children.Add(new ModulePropertyGridControl(moduleViewModel));
                        }
                    }
                }
            }
            else if (selectedObject is ListViewModel)
            {
                propertyGridPlaceholder.Children.Clear();
                propertyGridPlaceholder.Children.Add(new ListPropertyGridControl(selectedObject as ListViewModel));
            }

        }
    }
}
