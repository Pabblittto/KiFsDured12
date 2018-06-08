using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    public class Osoba : Klient
    {
        public  string Imie { get; set; }
        public string Nazwisko { get; set; }
        public Osoba(string imie,string nazwisko,string ID) :base(ID)
        {
            
            this.Imie = imie;
            this.Nazwisko = nazwisko;
        }
    }
}
