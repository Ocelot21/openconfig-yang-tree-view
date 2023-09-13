using openconfig_yang_tree_view.DataAccess;
using openconfig_yang_tree_view.MVVM.Model;
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
    /// Interaction logic for TreeViewView.xaml
    /// </summary>
    public partial class TreeViewView : UserControl
    {
        private InMemoryDb _database = DataAccessService.YangDatabase;
        public TreeViewView()
        {
            InitializeComponent();
            CreateTreeView();
        }

        public void CreateTreeView()
        {
            TreeView treeView = new TreeView();
            foreach (var module in _database.Modules) 
            {
                var treeViewItem = new TreeViewItem();
                Grouping root = null;
                if (module.Uses.Count>0)
                    root = module.Groupings.FirstOrDefault(g => g.Name == module.Uses.FirstOrDefault().Name);
                if (root != null)
                {
                    treeViewItem.Header = root.Name;
                    treeView.Items.Add(treeViewItem);
                }
                
            }      
            YangTreeView.Children.Add(treeView);
        }
    }
}
