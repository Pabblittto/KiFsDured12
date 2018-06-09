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
    /// Logika interakcji dla klasy DodajTypSamolotu.xaml
    /// </summary>
    public partial class DodajTypSamolotu : Window
    {
        Firma tmp;
        public DodajTypSamolotu(Firma Obiekt)
        {
            InitializeComponent();
            tmp = Obiekt;
        }

        private void DodajTypSamolotu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            tmp.PrzyciskDodajTypSamolotow(NazwaTextBox.Text,ZasiegTextBox.Text,PredkoscTextBox.Text,IloscMiejscTextBox.Text,IloscMiejscVIPTextBox.Text);
            }
            catch (Wyjatek)//łapie wszystkie wyjątki mojego napisanego typu
            {
                // tu ma sie wyświetlić okienko
            }
            
            // tu wywoła się funkcja dodania typu samolotu
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