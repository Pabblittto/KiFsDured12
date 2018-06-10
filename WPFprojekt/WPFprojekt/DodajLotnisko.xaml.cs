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
    /// Logika interakcji dla klasy DodajLotnisko.xaml
    /// </summary>
    public partial class DodajLotnisko : Window
    {
        Firma tmp;
        public DodajLotnisko(Firma ObiektFirmy)
        {
            InitializeComponent();
            tmp = ObiektFirmy;
        }
        private void DodajLotnisko_Click(object sender, RoutedEventArgs e)
        {
            try { tmp.PrzyciskDodajLotnisko(NazwaTextBox.Text); }
            catch(Wyjatek elo)
            {
                OkienkoBledy okienko = new OkienkoBledy(elo.Wiadomosc);
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
