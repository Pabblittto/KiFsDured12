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

        public DodajTrase()
        {
            InitializeComponent();
            listaLotnisk.Add(new Lotnisko("Lyndon"));
            listaLotnisk.Add(new Lotnisko("Berlon"));
            listaLotnisk.Add(new Lotnisko("Mokswa"));
            listaLotnisk.Add(new Lotnisko("Karkow"));
            listaLotnisk.Add(new Lotnisko("Martyd"));
            initBind();
        }
        private void initBind()
        {
            lista_Lotnisko.ItemsSource = listaLotnisk;
            lista_Lotnisko2.ItemsSource = listaLotnisk;
            CollectionView Lista1 = (CollectionView)CollectionViewSource.GetDefaultView(lista_Lotnisko.ItemsSource);
            CollectionView Lista2 = (CollectionView)CollectionViewSource.GetDefaultView(lista_Lotnisko2.ItemsSource);
            Lista1.Filter = NameFilter;
            Lista2.Filter = Name2Filter;
        }

        private bool NameFilter(object item)
        {
            if (String.IsNullOrEmpty(Nazwa1TextBox.Text))
                return true;
            else
                return ((item as Lotnisko).IDLotniska.IndexOf(Nazwa1TextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private bool Name2Filter(object item)
        {
            if (String.IsNullOrEmpty(Nazwa2TextBox.Text))
                return true;
            else
                return ((item as Lotnisko).IDLotniska.IndexOf(Nazwa2TextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void Nazwa1ZM(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lista_Lotnisko.ItemsSource).Refresh();
        }
        private void Nazwa2ZM(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lista_Lotnisko2.ItemsSource).Refresh();
        }



        private void DodajTrase_Click(object sender, RoutedEventArgs e)
        {
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
