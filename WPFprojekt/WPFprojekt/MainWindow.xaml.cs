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

namespace WPFprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Okno.Content = new Glowny();     
        }
        

        private void FunkcjaRezerwuj(object sender, RoutedEventArgs e)
        {
            Okno.Content = new Inny();
        }

        private void WyswietlGlowna(object sender, RoutedEventArgs e)
        {
            Okno.Content = new Glowny();
        }

        private void Dodaj_Lotnisko(object sender, RoutedEventArgs e)
        {
            Okno.Content = new DodawanieLotnisk(this);
        }

        private void Dodaj_Klienta(object sender, RoutedEventArgs e)
        {
            Okno.Content = new DodawanieKlientow(this);
        }
    }




}
