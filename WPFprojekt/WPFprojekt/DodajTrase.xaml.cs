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
        public List<Lotnisko> listaLotnisk = new List<Lotnisko>();
        Firma tmp;

        public DodajTrase(Firma Obiekt)
        {
            InitializeComponent();
            listaLotnisk.Add(new Lotnisko("Lyndon"));
            listaLotnisk.Add(new Lotnisko("Berlon"));
            listaLotnisk.Add(new Lotnisko("Mokswa"));
            listaLotnisk.Add(new Lotnisko("Karkow"));
            listaLotnisk.Add(new Lotnisko("Martyd"));
            initBind();
            tmp = Obiekt;// żeby te okienko widziało główną firmę
        }
        private void initBind()
        {
            lista_Lotnisko.ItemsSource = listaLotnisk;
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

        private void DodajTrase_Click(object sender, RoutedEventArgs e)
        {
           /* tmp.PrzyciskDodajTrase(lista_Lotnisko.SelectedItem,lista_Lotnisko2.SelectedItem,0) */ 

            // tu wywoła się funkcja dodania trasy
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
