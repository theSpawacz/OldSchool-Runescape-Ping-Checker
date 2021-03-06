﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


namespace OldSchool_Runescape_Ping_Checker
{
    
    public partial class MainWindow : Window
    {

        #region InitializationShit
        public static MainWindow Instance { get; private set; }
        


        //Inicjalizacja i magia
        
        

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            //After initialization
            Methods.SetColumns();
        }
        #endregion

        #region VariablesAndConstants
        internal static bool showMore = false;
        public static bool customString = false;
        

        internal static int ServerNumberToCheck = 115;
        internal static int BestPing = 99999;

        internal static List<System.Net.NetworkInformation.PingReply> ReplyList = new List<System.Net.NetworkInformation.PingReply>();        

        internal static Thread th1 = new Thread(Methods.CheckPing);


        #endregion


        #region Buttons

        //Check ping button
        private void MenuButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            
            
                if (th1.IsAlive)
                {
                Methods.Stop();

                    
                }
                else
                {
                    Methods.ChangeclickButton(1);
                th1 = new Thread(Methods.CheckPing);
                th1.Start();
                }

            
            

            
        }


        
        //SaveLogs
        private void MenuButtonSaveLogs_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            string tmp = dt.ToString("MM/dd/yyyy HH/mm/ss");
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt",
                FileName = "Oldschool Runescape Ping Log " + tmp
            };

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, TBLog.Text + Environment.NewLine + "Generated by OldSchool Runescape Ping Checker");

        }
       

        //About
        private void MenuButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            About ab = new About();
            ab.Show();
        }

        private void MenuButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings s = new Settings();
            s.Show();
        }
        //Close application
        private void MenuButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        //Event
        private void DGResults_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Methods.ColorGrid(e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
