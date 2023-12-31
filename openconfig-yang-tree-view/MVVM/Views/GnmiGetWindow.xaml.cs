﻿using openconfig_yang_tree_view.GnmiClient;
using openconfig_yang_tree_view.MVVM.ViewModels;
using System.Windows;

namespace openconfig_yang_tree_view.MVVM.Views
{
    /// <summary>
    /// Interaction logic for GnmiGetWindow.xaml
    /// </summary>
    public partial class GnmiGetWindow : Window
    {
        public GnmiGetWindow(TreeNodeViewModel node)
        {
            
            InitializeComponent();
            if (node is LeafViewModel leaf)
            {
                txtResponse.Text = GnmiClientRequests.GetRequest
                (Properties.Settings.Default.Ip,
                Properties.Settings.Default.Port,
                Properties.Settings.Default.Username,
                Properties.Settings.Default.Password,
                Properties.Settings.Default.IsHttps,
                leaf.Path);
            }
            else if (node is ContainerViewModel container)
            {
                txtResponse.Text = GnmiClientRequests.GetRequest
                (Properties.Settings.Default.Ip,
                Properties.Settings.Default.Port,
                Properties.Settings.Default.Username,
                Properties.Settings.Default.Password,
                Properties.Settings.Default.IsHttps,
                container.Path);
            }
            else if (node is ListViewModel list)
            {
                txtResponse.Text = GnmiClientRequests.GetRequest
                (Properties.Settings.Default.Ip,
                Properties.Settings.Default.Port,
                Properties.Settings.Default.Username,
                Properties.Settings.Default.Password,
                Properties.Settings.Default.IsHttps,
                list.Path);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtResponse.Text))
            {
                // Copy the text to the clipboard
                Clipboard.SetText(txtResponse.Text);
                MessageBox.Show("Text copied to clipboard.", "Copy Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No text to copy.", "Copy Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
