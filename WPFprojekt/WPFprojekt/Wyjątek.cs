using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    /// <summary>
    /// wyjątek do programu jeden wystarczy do wielu rzeczy- jak się umiejętnie korzysta
    /// </summary>
    class Wyjatek : Exception
    {
        public string Wiadomosc;

        public Wyjatek(string co_sie_stao)
        {
            Wiadomosc = co_sie_stao;
        }

        public override String ToString()
        {
            return Wiadomosc;
        }

    }
}
