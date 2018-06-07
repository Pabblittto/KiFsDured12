using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{/// <summary>
/// Klasa- przepis na lot cykliczny
/// </summary>
    public class PlanLotu
    {
        private DateTime CZasBazowy;// Czas przechowuje date ostatniego lotu który stworzył 
        public TimeSpan CoIleLata { get; set; }// co ile lata dany lot , najlepiej okrągłe wartości
        public TimeSpan NaJakiPrzedzialczasu { get; set; }// może tworzyć automatycznie loty na najbliższy tydzien na przykład- zawsze na najbliższy tydzień jest samolot
        public Trasa Polaczenie { get; set; }
        public TypSamolotu RodzajSamolotu { get; set; }
        public Samolot Pojazd{ get; set; }

    public PlanLotu(DateTime PierwszyLot,TimeSpan _CoIleLata,Trasa Kierunek,TypSamolotu _RodzajSamolotu, TimeSpan NajakiPrzedzialCzasuTworzyc, Samolot _Pojazd )
        {
            NaJakiPrzedzialczasu = NajakiPrzedzialCzasuTworzyc;
            Polaczenie = Kierunek;
            CZasBazowy = PierwszyLot;
            CoIleLata = _CoIleLata;
            RodzajSamolotu = _RodzajSamolotu;
            PierwszyLot = PierwszyLot.Subtract(CoIleLata);// te odjęcie czasu wiąże się z sposobem dodawania nowych lotów
            Pojazd = _Pojazd;
        }

        /// <summary>
        /// Metoda statyczna , Po to żeby sprawdzać czy wybrany samolot i trasa będą dobre- metode stayczną da się wywoływać jak nie istnieje obiekt
        /// </summary>
        /// <param name="TypPojazdu"></param>
        /// <param name="Droga"></param>
        public static Boolean CzyDoleci(TypSamolotu TypPojazdu, Trasa Droga)
        {
            if (TypPojazdu.GetZasieg() >= Droga.GetOdleglosc())
                return true;
            else
                return false;
        }

        public Boolean CzyTrzebaStworzyc(DateTime _AktualnaData)
        {
            if (CZasBazowy.Add(CoIleLata) <= _AktualnaData.Add(NaJakiPrzedzialczasu))
                return true;            
            else
                return false;
        }


        public void CzyzdarzyWrocic()
        {





        }


        /// <summary>
        /// Sprawdza czy trzeba dodać loty do Glownej listylotow
        /// </summary>
        /// <param name="AktualnaData"></param>
        /// <returns></returns>
        public List<Lot> StworzLotyCykliczne(DateTime AktualnaData )
        {
            List<Lot> ListaLotowKtoraTrzebaDodac = new List<Lot>();
            while(CzyTrzebaStworzyc(AktualnaData))
            {
                CZasBazowy = CZasBazowy.Add(CoIleLata);
                ListaLotowKtoraTrzebaDodac.Add(new Lot("0", Polaczenie, CZasBazowy.Year, CZasBazowy.Month, CZasBazowy.Day, CZasBazowy.Hour, CZasBazowy.Minute, true));// ID Lotu musi zostac zmienione przez funkcje w Firmie
            }
            return ListaLotowKtoraTrzebaDodac;
        }


    }
}
