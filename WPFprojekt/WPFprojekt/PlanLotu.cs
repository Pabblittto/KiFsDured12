using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{/// <summary>
/// Klasa- przepis na lot cykliczny
/// </summary>
/// 
        [Serializable]
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
            Pojazd.Cykliczny = true;
            
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
        /// <summary>
        /// Funkcja liczy ile samolot leci w jedną stronę
        /// </summary>
        /// <returns></returns>
        public static TimeSpan ILeleciWjednaStrone(TypSamolotu TypSamolotuKtorychcemydodac,Trasa TrasaCochcemydodac)
        {
            TimeSpan CzasLotu;
            double czas = TrasaCochcemydodac.GetOdleglosc() / TypSamolotuKtorychcemydodac.GetPredkosc();// czas wychodzi w godz z minutamie po przecinku
            czas = Math.Round(czas, 2);
            double min = (czas % 1) * 60;// minuty w formiacie 0,xx więc trzeba pomnożyć razy 60
            CzasLotu = new TimeSpan((int)czas, (int)min, 0);// zero na końcu- to sekundy nieistotne w programie
            return CzasLotu;
        }

        /// <summary>
        /// Zwraca true jeżeli samolot zdąrzy rócić przed nastepnym lotem, wywala wyjatek jeżeli nie ma wystarczająco dużo pul wypełnionych
        /// więc wystarczy ładnie obsłurzyć
        /// </summary>
        public static Boolean CzyzdarzyWrocic(TypSamolotu TypSamolotuKtorychcemyDodac,Trasa TrasaCochemydodac, TimeSpan CoIleMaLatac)// ta funkcja jest do poprawy musi być statyczna
        {
            if (TypSamolotuKtorychcemyDodac != null && TrasaCochemydodac != null && CoIleMaLatac != null)
            {
                TimeSpan CalkowityCzas = PlanLotu.ILeleciWjednaStrone(TypSamolotuKtorychcemyDodac, TrasaCochemydodac);
                CalkowityCzas= CalkowityCzas.Add(CalkowityCzas);
                CalkowityCzas = CalkowityCzas.Add(new TimeSpan(3,0,0));
                if (CalkowityCzas.CompareTo(CoIleMaLatac) >= 0)
                    return false;
                else
                    return true;
            }
            else
                throw new Wyjatek("Dodaj wymagane pola!!");

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
                ListaLotowKtoraTrzebaDodac.Add(new Lot("0", Polaczenie, CZasBazowy, RodzajSamolotu,Pojazd,this));// ID Lotu musi zostac zmienione przez funkcje w Firmie
                ListaLotowKtoraTrzebaDodac.Add(new Lot("0", new Trasa(Polaczenie), CZasBazowy.Add(PlanLotu.ILeleciWjednaStrone(RodzajSamolotu,Polaczenie).Add(new TimeSpan(3,0,0))),RodzajSamolotu,Pojazd,this));
            }
            return ListaLotowKtoraTrzebaDodac;
        }


    }
}
