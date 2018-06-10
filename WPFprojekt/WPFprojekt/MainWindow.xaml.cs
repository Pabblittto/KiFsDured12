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
using System.Windows.Threading;

namespace WPFprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Firma GlownaFirma = new Firma();


        public MainWindow()
        { 
            InitializeComponent();
            GlownaFirma.OdczytZPliku();// Funkcja odpale się na początku tylko  raz

            Okno.Content = new Glowny();

            InitBinding();

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        ~MainWindow()// destruktor okna
        {
            GlownaFirma.ZapisDoPliku();//zapis do pliku kiedy jest wywalane okno
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            GlownaFirma.PrzedawnianieRezerwacji();
            GlownaFirma.WyslijWKosmos();
            GlownaFirma.SprawdzanieStanuLotow();
            GlownaFirma.SprawdzenieStanuOdbytychLotow();
            GlownaFirma.BlokujRezerwacje();
            Data.Text = GlownaFirma.WirtualnaDataAktualzacja();
        }

        private void InitBinding()
        {
            GlownaFirma.AktualizacjaLotowCyklicznych();
            Data.DataContext = GlownaFirma;
        }
        

        private void FunkcjaRezerwuj(object sender, RoutedEventArgs e)
        {
            Okno.Content = new Inny();
        }

        private void WyswietlGlowna(object sender, RoutedEventArgs e)
        {
            Okno.Content = new Glowny();
        }



        private void Okno_DodajKlienta(object sender, RoutedEventArgs e)
        {
            DodajKlienta oknoKlient = new DodajKlienta();
            oknoKlient.Owner = this;
            oknoKlient.ShowDialog();
            oknoKlient = null;
        }
        private void Okno_DodajPosrednika(object sender, RoutedEventArgs e)
        {
            DodajPosrednika oknoPosrednik = new DodajPosrednika(GlownaFirma);
            oknoPosrednik.Owner = this;
            oknoPosrednik.ShowDialog();
            oknoPosrednik = null;
        }
        private void Okno_DodajLotnisko(object sender, RoutedEventArgs e)
        {
            DodajLotnisko oknoLotnisko = new DodajLotnisko(GlownaFirma);
            oknoLotnisko.Owner = this;
            oknoLotnisko.ShowDialog();
            oknoLotnisko = null;
        }
        private void Okno_DodajTypSamolotu(object sender, RoutedEventArgs e)
        {
            DodajTypSamolotu oknoTypSamolotu = new DodajTypSamolotu(GlownaFirma);
            oknoTypSamolotu.Owner = this;
            oknoTypSamolotu.ShowDialog();
            oknoTypSamolotu = null;
        }
        private void Okno_DodajSamolot(object sender, RoutedEventArgs e)
        {
            DodajSamolot oknoSamolot = new DodajSamolot(GlownaFirma);
            oknoSamolot.Owner = this;
            oknoSamolot.ShowDialog();
            oknoSamolot = null;
        }
        private void Okno_DodajTrase(object sender, RoutedEventArgs e)
        {
            DodajTrase oknoTrasa = new DodajTrase(GlownaFirma);
            oknoTrasa.Owner = this;
            oknoTrasa.ShowDialog();
            oknoTrasa = null;
        }
    }
}
