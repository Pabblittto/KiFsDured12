using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logika interakcji dla klasy DodajTrase.xaml
    /// </summary>
    public partial class DodajTrase : Window
    {
        Firma tmp;

        public DodajTrase(Firma Obiekt)
        {
            InitializeComponent();
            tmp = Obiekt;// żeby te okienko widziało główną firmę
            initBind();
        }
        private void initBind()
        {
            lista_Lotnisko.ItemsSource = tmp.ListaLotnisk;
            CollectionView Lista1 = (CollectionView)CollectionViewSource.GetDefaultView(lista_Lotnisko.ItemsSource);
            Lista1.Filter = NameFilter;
        }
        private bool NameFilter(object item)
        {
            if (String.IsNullOrEmpty(NazwaTextBox.Text))
                return true;
            else
                return ((item as Lotnisko).IDLotniska.IndexOf(NazwaTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void NazwaSzukana(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lista_Lotnisko.ItemsSource).Refresh();
        }

        private void lista_Lotnisko_Zmiana(object sender, SelectionChangedEventArgs e)
        {
            if (lista_Lotnisko.SelectedItems.Count == 2) Dodaj.IsEnabled = true;
            else Dodaj.IsEnabled = false;
        }



        private void DodajTrase_Click(object sender, RoutedEventArgs e)
        {
            List<Lotnisko> ListaLotnisk = new List<Lotnisko>();
            foreach (Lotnisko lotnisko in lista_Lotnisko.SelectedItems)
            {
                ListaLotnisk.Add(lotnisko);
            }
            try { tmp.PrzyciskDodajTrase(ListaLotnisk[0], ListaLotnisk[1], Convert.ToUInt32(OdlegloscTextBox.Text)); }
            catch (Wyjatek elo)
            {
                OkienkoBledy okienko = new OkienkoBledy(elo.Wiadomosc);
                okienko.Owner = this;
                okienko.ShowDialog();
                okienko = null;
            }
            catch(FormatException)
            {
                OkienkoBledy okienko = new OkienkoBledy("Wpisz liczbę w odległości!!");
                okienko.Owner = this;
                okienko.ShowDialog();
                okienko = null;
            }
            catch(OverflowException)
            {
                OkienkoBledy okienko = new OkienkoBledy("Liczba powinna być dodatnia!!");
                okienko.Owner = this;
                okienko.ShowDialog();
                okienko = null;
            }
            this.DialogResult = true;
            this.Close();
        }
        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
