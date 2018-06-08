using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
     public class TypSamolotu
    {
        public string NazwaModelu { get; set; }

        public int Zasieg { get; set; }// podawana w km
        public int Predkosc { get; set; }// prędkośc podawana w km/h
        public int IloscMiejsc { get; set; }
        public int IloscMiejscVIP { get; set; }

        private List<Samolot> ListaSamolotow;
        private List<string> LNIDSamolotow;



        public TypSamolotu(string Nazwa,int _Zasieg, int _Predkosc, int IlMiejsc, int IlMiejscVip)
        {
            LNIDSamolotow = new List<string>();
            NazwaModelu = Nazwa;
            Zasieg = _Zasieg;
            Predkosc = _Predkosc;
            IloscMiejsc = IlMiejsc;
            IloscMiejscVIP = IlMiejscVip;
            ListaSamolotow = new List<Samolot>();
        }

        public string GetNazwaModelu()
        {
            return NazwaModelu;
        }

        public int GetIloscMiejsc()
        {
            return IloscMiejsc;
        }

        public int GetPredkosc()
        {
            return Predkosc;
        }

        public int GetIloscMiejscVip()
        {
            return IloscMiejscVIP;
        }

        public int GetZasieg()
        {
            return Zasieg;
        }
        public List<Samolot> GetListaSamolotow()
        {
            return ListaSamolotow;
        }

        public List<string> GetLNIDSamolotow()
        {
            return LNIDSamolotow;
        }
        /// <summary>
        /// Funkcja dodajaca sammolot zwraca false jeżeli samolot z takim samym id znajduje sie na liście 
        /// w przeciwnym wypadku zwraca true
        /// </summary>
        /// <param name="Dodawany"></param>
        /// <returns></returns>
        public Boolean DodajSamolot(string IDSamolotu)
        {
            if (ListaSamolotow.Count() != 0)
            {
                foreach (Samolot Obiekt in ListaSamolotow)
                {
                    if (Obiekt.IDObiektu == IDSamolotu)
                        return false;
                }
            }
            //mala zmiana - lepiej jak w nizsszej metodzie sprawdzac tylko ID bo nowy samolot i tak bedzie mial takie same parametry a roznil sie tylko ID
            ListaSamolotow.Add(new Samolot(IDSamolotu));
            return true;
        }

        /// <summary>
        /// Funkcja usuwa z listy samoolot który ma dane id, jeżeli dany samolot istnieje na liście - zwraca true
        /// w przeciwnym wypadku zwraca false
        /// </summary>
        /// <param name="IDSamolotu"></param>
        /// <returns></returns>
        public Boolean UsunSamolot(string IDSamolotu)
        {
            if (ListaSamolotow.Count() != 0)
            {
                for (int i = 0; i < ListaSamolotow.Count(); i++)
                {
                    if (ListaSamolotow[i].IDObiektu == IDSamolotu)
                    {
                        ListaSamolotow.Remove(ListaSamolotow[i]);

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// funkcja zwraca obiekt Samolotu o podanym id - funkcja może się przydac żeby sprawdzić czy 
        /// dany samolot jest wolny, zwraca nulla jeżeli tego id nie ma na liście 
        /// </summary>
        /// <param name="IDszukanego"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public Samolot GetSAmolotOID(string IDszukanegoSamolotu)
        {
            foreach (Samolot Obiekt in ListaSamolotow)
            {
                if (Obiekt.IDObiektu == IDszukanegoSamolotu)
                    return Obiekt;
            }
            throw new Wyjatek("Nie ma takiego Samolotu o podanym ID na liście!! ");// niech użytkownik wpisze te ID jeszcze raz, jeżeli ma możliwość wgl
        }
        // jeżeli funkcja będzie sprawiała problemy można ją wywalić :/

    }
}
