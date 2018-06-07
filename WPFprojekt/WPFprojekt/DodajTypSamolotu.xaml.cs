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
    /// Logika interakcji dla klasy DodajTypSamolotu.xaml
    /// </summary>
    public partial class DodajTypSamolotu : Window
    {
        public DodajTypSamolotu()
        {
            InitializeComponent();
        }
        private void DodajTypSamolotu_Click(object sender, RoutedEventArgs e)
        {
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