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
    /// Interaction logic for OkienkoBledy.xaml
    /// </summary>
    public partial class OkienkoBledy : Window
    {
        public OkienkoBledy(String nazwabledu)
        {
           
            InitializeComponent();
            Informacja.Text = nazwabledu;
        }

        private void Okejka_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
