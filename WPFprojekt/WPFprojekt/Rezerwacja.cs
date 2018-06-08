using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    /// <summary>
    /// Klasa która jednocześnie jest rezerwacją i biletem
    /// </summary>
    public class RezerwcjaBilet
    {
        public Klient Pasazer;
        public Lot Polaczenie;

        public string NrRezerwacjiBiletu { get; set; }
        public uint NrMiesca { get; set; }
        public int CenaBiletu { get; set; }
        public Boolean BiletVIP { get; set; }
        public Boolean CzyKupionyBilet { get; set; }

        public DateTime DataWygasniecia { get; set; }

        // ostatni element w konstruktorze decyduje obiket stajie sie od razu biletem
        public RezerwcjaBilet(string NrRezer,int _Cena,Boolean Vip, Klient KtoRezerw,DateTime Datastworzenia,Boolean _CzyKupionyBilet,Lot _Polaczenie )
        {
            Polaczenie = _Polaczenie;
            Pasazer = KtoRezerw;
            NrRezerwacjiBiletu = NrRezer;
            NrMiesca = (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + NrRezer.Split('-')[1]);
            CenaBiletu = _Cena;
            BiletVIP = Vip;
            DataWygasniecia = Datastworzenia.Add(new TimeSpan(7, 0, 0, 0));// czas rezerwacji , rezerwacje można zrobić tylko na 7 dni                                                        // jeżeli data wygaśniećia bedzie się równać dacie w programie to rezerwacja jest usuwana z listy rezerwacji
            CzyKupionyBilet = _CzyKupionyBilet;
        }


        public string GetNrRezerwacji()
        {
            return NrRezerwacjiBiletu;
        }

        public Boolean CzyVIP()
        {
            return BiletVIP;
        }


        /// <summary>
        /// Może się przyda ta funkcja , nie wiem , zwraca czy dany obiekt jest rezerwacją czy biletem
        /// </summary>
        /// <returns></returns>
        public Boolean CzyJestToBilet()
        {
            return CzyKupionyBilet;
        }

        /// <summary>
        /// Funkcja zwracająca czy data wygaścnięcia mineła czy nie, zwraca true jeżeli rezerwacja wygasła.
        /// false- nie wygasła jeszcze- przyda się jak trzeba wywalac rezerwacje po czasie wygaśnięcia
        /// </summary>
        /// <param name="DatawProgramie">Wirtualna data w programie</param>
        /// <returns></returns>
        public Boolean CzyWygaslo(DateTime DataWProgramie)
        {
            if (CzyKupionyBilet == false)
            {
                if (DataWygasniecia.CompareTo(DataWProgramie) <= 0)
                    return true;
                else
                    return false;
            }
            else
               return false;
        }

    }
}
