using openconfig_yang_tree_view.Services;
using System.Text;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace openconfig_yang_tree_view.MVVM.Views
{
    /// <summary>
    /// Interaction logic for FilesView.xaml
    /// </summary>
    public partial class FilesView : UserControl
    {
        private ParsingService _parsingService = new ParsingService();
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
            var (filesList, missingFiles) = _parsingService.ParseFromFolder(txtFolderPath.Text);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbMissing = new StringBuilder();
            foreach (var file in filesList)
            {
                sb.AppendLine($"{file}");
            }
            foreach (var file in missingFiles)
            {
                sbMissing.AppendLine($"{file}");
            }
            txtParsed.Text = sb.ToString();
            txtMissing.Text = sbMissing.ToString();
        }
    }
}
