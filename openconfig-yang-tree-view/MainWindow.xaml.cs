using openconfig_yang_tree_view.MVVM.Views;
using System.Windows;
using System.Windows.Forms;

namespace openconfig_yang_tree_view
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FilesView FilesView { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            FilesView = new FilesView();
            contentControl.Content = FilesView;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = FilesView;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new YangTreeView();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new SettingsView();
        }
    }
}
