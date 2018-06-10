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

namespace WPFprojekt
{
    /// <summary>
    /// Logika interakcji dla klasy ZarzadzajTrasami.xaml
    /// </summary>
    public partial class ZarzadzajTrasami : Window
    {
        Firma tmp;

        public ZarzadzajTrasami(Firma Obiekt)
        {
            InitializeComponent();
            tmp = Obiekt;// żeby te okienko widziało główną firmę
            initBind();
        }
        private void initBind()
        {
            lista_Trasa.ItemsSource = tmp.ListaTras;
            CollectionView Lista1 = (CollectionView)CollectionViewSource.GetDefaultView(lista_Trasa.ItemsSource);
            Lista1.Filter = NameFilter;
        }
        private bool NameFilter(object item)
        {
            if (String.IsNullOrEmpty(NazwaTextBox.Text))
                return true;
            else
                return ((item as Trasa).IDTrasy.IndexOf(NazwaTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void NazwaSzukana(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lista_Trasa.ItemsSource).Refresh();
        }

        private void DodajTrase_Click(object sender, RoutedEventArgs e)
        {
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
