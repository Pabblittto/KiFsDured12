﻿using System;
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
        public DodajLotnisko()
        {
            InitializeComponent();
        }
        private void DodajLotnisko_Click(object sender, RoutedEventArgs e)
        {
            // tu wywoła się funkcja dodania lotniska
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
