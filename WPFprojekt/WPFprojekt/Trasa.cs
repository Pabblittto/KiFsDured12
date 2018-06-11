using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    /// <summary>
    /// Trasa to obiekt przechowyjący dwa lotniska, jest to po prostu droga
    /// </summary>
    [Serializable]
    public class Trasa
    {
        public Lotnisko Lotnisko1 { get; set; }
        public Lotnisko Lotnisko2 { get; set; }

        public int Odleglosc { get; set; }// podawana w km

        public string IDTrasy { get; set; }

        public Trasa(Lotnisko Lot1, Lotnisko Lot2, int odleg)
        {
            IDTrasy = Lot1.GetIDLotniska() + "-" + Lot2.GetIDLotniska();
            Lotnisko1 = Lot1;
            Lotnisko2 = Lot2;
            Odleglosc = odleg;
        }

        /// <summary>
        /// konstruktor kopiujący, który odwraca kierunek poróży
        /// </summary>
        /// <param name="Droga"></param>
        public Trasa(Trasa Droga) 
        {
            this.Lotnisko1 = Droga.Lotnisko2;
            this.Lotnisko2 = Droga.Lotnisko1;
            Odleglosc = Droga.Odleglosc;
            this.IDTrasy = Lotnisko1.GetIDLotniska() + "-" + Lotnisko2.GetIDLotniska();
        }

        public int GetOdleglosc()
        {
            return Odleglosc;
        }

        public string GetIDTrasy()
        {
            return IDTrasy;
        }

        


    }
}
