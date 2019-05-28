using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace OldSchool_Runescape_Ping_Checker
{
    public struct Result
    {
        public int Number { set; get; }
        public long Delay { set; get; }
        public int Tl { set; get; }
        public bool Fr { set; get; }
        public string Bf { set; get; }




    }

    class Methods
    {

        #region Misc

        internal static void ColorGrid(System.Windows.Controls.DataGridRowEventArgs e)
        {

            Result obj = (Result)e.Row.Item;
            var converter = new System.Windows.Media.BrushConverter();
            if (obj.Delay <= MainWindow.BestPing) e.Row.Background = Brushes.Aqua;
            else if (obj.Delay <= 75) e.Row.Background = Brushes.GreenYellow;
            else if (obj.Delay > 75 && obj.Delay <= 150) e.Row.Background = (Brush)converter.ConvertFromString("#FFFFFF90");
            else
                e.Row.Background = (Brush)converter.ConvertFromString("#ff3300");

        }

        internal static void ChangeclickButton(int v)
        {
            //0 = thread not running
            if (v == 0)
            {
                MainWindow.Instance.MenuButtonCheck.Header = "Check Ping!";
                MainWindow.Instance.MenuButtonCheck.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA1EEA8"));

            }
            else
            {
                MainWindow.Instance.MenuButtonCheck.Header = "Stop";
                MainWindow.Instance.MenuButtonCheck.Background = Brushes.Red;
            }

        }
        internal static void SetColumns()
        {
            string[] tabBinding = { "Number", "Delay", "Tl", "Fr", "Bf" };
            string[] tabHeader = { "Number", "Ping", "TL", "Don't fragment", "Buff size" };            

            for (int i = 0; i < 5; i++)
            {
                DataGridTextColumn dgtc = new DataGridTextColumn
                {
                    Binding = new Binding(tabBinding[i]),
                    Header = tabHeader[i]
                };
                MainWindow.Instance.DGResults.Columns.Add(dgtc);
            }
        }
        #endregion

        #region Pinging

        internal static void CheckPing()
        {
            

            try
            {
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                    //czyscimy 
                    MainWindow.Instance.DGResults.Items.Clear();
                    MainWindow.ReplyList.Clear();
                }));

                Pinging();
            }
            catch(Exception e)
            {
                InvokeStatusBar(e.Message);
            }
            
            finally
            {
                    //Invoke
                    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                        ChangeclickButton(0);
                    }));
                
            }
            


        }

        private static void Pinging()
        {


            string toCheck;
            //Start from 1
            for (int i= 1;i < MainWindow.ServerNumberToCheck +1; i++)
            {

                toCheck = "oldschool" + i + ".runescape.com";
                Ping pingSender = new Ping();
                string tmps = "";
                
                IPAddress[] ips = Dns.GetHostAddresses(toCheck);

                foreach (var x in ips)
                {
                    tmps += x.ToString();
                }

                PingReply PR = pingSender.Send(tmps);
                MakeLog(PR, i);
                MainWindow.ReplyList.Add(PR);
                

            }

            SearchForLowestPing();
            LoadToList();




        }

        private static void SearchForLowestPing()
        {
            long Lowest = 9999;
            foreach(var i in MainWindow.ReplyList)
            {
                if (i.RoundtripTime <= Lowest)
                    Lowest = i.RoundtripTime;
            }

            MainWindow.BestPing = (int)Lowest;
        }

        private static void LoadToList()
        {
            
            
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                for (int i = 0; i < MainWindow.ReplyList.Count - 1; i++)
                    MainWindow.Instance.DGResults.Items.Add(new Result() {
                    Number = i + 300,
         Delay = MainWindow.ReplyList[i].RoundtripTime,
         Tl = MainWindow.ReplyList[i].Options.Ttl,
         Fr = MainWindow.ReplyList[i].Options.DontFragment,
         Bf = MainWindow.ReplyList[i].Buffer.Length.ToString()


                }
                
                
                
                );
            }));
            
           
        }

        internal static void Stop()
        {
            MainWindow.th1.Abort();
            InvokeStatusBar("Zatrzymano proces przez użytkownika!");
            SearchForLowestPing();
            LoadToList();

            //Invoke
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                ChangeclickButton(0);

            }));
        }

        
        #endregion

        #region LogsAndViews

        private static void MakeLog(PingReply reply, int i)
        {
            i += 300;
            //Invoke
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                TextBlock TB = MainWindow.Instance.TBLog;
                if (MainWindow.showMore)
                {
                    TB.Text += "Server number: " + i + "..." + "\tDelay: " + reply.RoundtripTime.ToString() + "ms | TL:  " + reply.Options.Ttl +  "\t\t\tDon't fragment: " +
                    reply.Options.DontFragment + "\t\tBuffer size: " + reply.Buffer.Length + Environment.NewLine;

                }
                else
                {
                    TB.Text += "Server number: " + i + "..." + "\tDelay: " + reply.RoundtripTime.ToString() + Environment.NewLine;
                }
                MainWindow.Instance.SC.ScrollToBottom();
            }));

        }

        private static void InvokeStatusBar(string v)
        {
            //Invoke
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
                MainWindow.Instance.Statusbar.Content = v;
            }));
            
        }


        #endregion

    }
}
