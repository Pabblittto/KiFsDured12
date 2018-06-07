using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    public class Posrednik : Klient
    {
        private string Nazwa;
        public Posrednik(string nazwa,string ID) :base(ID)
        {
            this.Nazwa = nazwa;
        }
        public string GetNazwa()
        {
            return Nazwa;
        }
    }
}