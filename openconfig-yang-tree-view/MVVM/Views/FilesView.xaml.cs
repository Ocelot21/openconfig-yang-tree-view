using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace openconfig_yang_tree_view.MVVM.Views
{
    /// <summary>
    /// Interaction logic for FilesView.xaml
    /// </summary>
    public partial class FilesView : UserControl
    {
        public FilesView()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var folderBrowserDialog = new WinForms.FolderBrowserDialog())
            {
               var dialogResult =  folderBrowserDialog.ShowDialog();

                if (dialogResult == WinForms.DialogResult.OK)
                {
                    txtFolderPath.Text = folderBrowserDialog.SelectedPath;
                    btnParse.IsEnabled = true;
                }
            }
        }

        private void btnParse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
    }
}
