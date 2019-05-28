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
using System.Windows.Shapes;

namespace OldSchool_Runescape_Ping_Checker
{
    /// <summary>
    /// Logika interakcji dla klasy Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            LoadSettings();
        }

        private void LoadSettings()
        {
            //Slider
            SliderValue.Value = MainWindow.ServerNumberToCheck;
            TBValue.Text = MainWindow.ServerNumberToCheck.ToString();

            //Checkboxes
            if (MainWindow.showMore)
                CBInfo.IsChecked = true;
            if (MainWindow.customString)
                CBAnotherString.IsChecked = true;
                     
    }

        #region CheckboxesAndSlider
        private void SliderValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            
            if (this.IsLoaded)//null exception
            {
                MainWindow.ServerNumberToCheck = Int32.Parse(SliderValue.Value.ToString());
                TBValue.Text = MainWindow.ServerNumberToCheck.ToString();
            }
            
        }

        private void CBAnotherString_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.customString = false;
            TBString.IsEnabled = false;
        }

        private void CBAnotherString_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.customString = true;
            TBString.IsEnabled = true;
        }




        private void CBInfo_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.showMore = false;
        }

        private void CBInfo_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.showMore = true;
        }

        #endregion
    }
}
