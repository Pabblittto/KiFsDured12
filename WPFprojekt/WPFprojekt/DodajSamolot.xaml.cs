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
    /// Logika interakcji dla klasy DodajSamolot.xaml
    /// </summary>
    public partial class DodajSamolot : Window
    {
        public List<TypSamolotu> listaTest = new List<TypSamolotu>();
        Firma tmp;

        public DodajSamolot(Firma Obiekt)
        {
            InitializeComponent();
            listaTest.Add(new TypSamolotu("Ajirbuc", 100, 100, 100, 100));
            listaTest.Add(new TypSamolotu("Bojeng", 101, 101, 101, 101));
            listaTest.Add(new TypSamolotu("Tuplowew", 101, 101, 101, 101));
            initBind();
            tmp = Obiekt;// żeby te okienko widziało główną firmę
        }
        private void initBind()
        {
            lista_TypSamolotu.ItemsSource = listaTest;
            CollectionView Lista1 = (CollectionView)CollectionViewSource.GetDefaultView(lista_TypSamolotu.ItemsSource);
            Lista1.Filter = NameFilter;
        }
        private bool NameFilter(object item)
        {
            if (String.IsNullOrEmpty(NazwaTextBox.Text))
                return true;
            else
                return ((item as TypSamolotu).NazwaModelu.IndexOf(NazwaTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void NazwaSzukana(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lista_TypSamolotu.ItemsSource).Refresh();
        }

        private void DodajSamolot_Click(object sender, RoutedEventArgs e)
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
