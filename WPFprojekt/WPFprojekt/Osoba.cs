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
        private string Imie;
        private string Nazwisko;
        public Osoba(string imie,string nazwisko,string ID) :base(ID)
        {
            
            this.Imie = imie;
            this.Nazwisko = nazwisko;
        }
        public string GetImie()
        {
            return Imie;
        }
        public string GetNazwisko()
        {
            return Nazwisko;
        }

    }
}
