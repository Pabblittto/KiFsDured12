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
