﻿using Adit.Code.Server;
using Adit.Code.Shared;
using Adit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Adit.Pages
{
    /// <summary>
    /// Interaction logic for ServerMain.xaml
    /// </summary>
    public partial class Server : Page
    {
        public static Server Current { get; set; }
        public Server()
        {
            InitializeComponent();
            Current = this;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshUI();
        }
        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = !(sender as Button).ContextMenu.IsOpen;
        }


        private void ImageCreateClientInfo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Clients must be created from this screen in order to use this server.\r\n\r\nThe connection information is embedded in the client EXE so there is no configuration required for the end user.\r\n\r\nIf you update the host or port, you must create new clients.", "Create Client", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ToggleServerStatus_Click(object sender, MouseButtonEventArgs e)
        {
            if (Code.Server.AditServer.IsEnabled)
            {
                e.Handled = true;
                var result = MessageBox.Show("Your server is currently running.  Are you sure you want to stop it?", "Confirm Shutdown", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                AditServer.Stop();
                toggleServerStatus.IsOn = false;
            }
            else
            {
                AditServer.Start();
            }
        }

        private void RefreshUI()
        {
            toggleServerStatus.IsOn = AditServer.IsEnabled;
            toggleSSL.IsOn = Config.Current.IsEncryptionEnabled;
            textHost.Text = Config.Current.ServerHost;
            textPort.Text = Config.Current.ServerPort.ToString();
            buttonConnectedClients.Content = AditServer.ClientCount.ToString();
        }
        // To refresh UI from other threads.
        public void RefreshUICall()
        {
            this.Dispatcher.Invoke(() => RefreshUI());
        }

        private void AuthenticationKeys_Click(object sender, RoutedEventArgs e)
        {
            var win = new Windows.AuthenticationKeys();
            win.Owner = MainWindow.Current;
            win.ShowDialog();
        }

        private void ConnectedClients_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.mainFrame.Navigate(new Pages.Hub());
        }

        private void ToggleSSL_Click(object sender, MouseButtonEventArgs e)
        {
            Config.Current.IsEncryptionEnabled = !toggleSSL.IsOn;
            if (Config.Current.IsEncryptionEnabled)
            {
                var win = new Windows.SSL();
                win.Owner = MainWindow.Current;
                win.ShowDialog();
            }
        }
    }
}
