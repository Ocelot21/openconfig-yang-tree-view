using openconfig_yang_tree_view.DataAccess;
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
        private InMemoryDb _dataBase = DataAccessService.YangDatabase;
        private TreeViewService _treeViewService = new TreeViewService();
        private TreeViewModel _tree = new TreeViewModel();

        public YangTreeView()
        {
            InitializeComponent();
            _treeViewService.FillTree(_tree);
            DataContext = _tree;
        }
    }
}
