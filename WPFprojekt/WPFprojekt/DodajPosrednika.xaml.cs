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
    /// Logika interakcji dla klasy DodajPosrednika.xaml
    /// </summary>
    public partial class DodajPosrednika : Window
    {
        Firma tmp;

        public DodajPosrednika(Firma Obiekt)
        {
            InitializeComponent();
            tmp = Obiekt;
        }
        private void DodajPosrednika_Click(object sender, RoutedEventArgs e)
        {
            // tu wywoła się funkcja dodania klienta
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
