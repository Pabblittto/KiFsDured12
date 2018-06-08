using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    public abstract class Klient : KlasaID
    {

        private List<RezerwcjaBilet> ListaBiletowRezerwacji;// lista biletów i rezerwacji

        public Klient(string ID)
            {
            ListaBiletowRezerwacji = new List<RezerwcjaBilet>();
            SetID(ID);
            }


        public List<RezerwcjaBilet> GetListaBiletowRezerwacji()
        {
            return ListaBiletowRezerwacji;
        }

        /// <summary>
        /// Funkcja zwracająca fałsz jak chcemy dodać bilet który już jest na liśćie
        /// nie wiem czy się przyda ale tak na wszelki wypadek już jest XD
        /// </summary>
        /// <returns></returns>
        public Boolean DodajBiletRezerwacje(RezerwcjaBilet DodawanyBilet) 
        {
            if(ListaBiletowRezerwacji.Count() !=0)
            {
                foreach (RezerwcjaBilet Obiekt in ListaBiletowRezerwacji)// to prawdopodobnie nie jest potrzebne , jak będziemy robili to w WPFie to nie będzie sytuacji gdzie dodajemy ten sam bilet
                {
                    if (Obiekt ==DodawanyBilet)
                        return false;
                }
            }
            ListaBiletowRezerwacji.Add(DodawanyBilet);
            return true;
        }

        /// <summary>
        /// Funkcja do usuwania biletów, zwraca fałsz jeżeli nie ma danego biletu na liście 
        /// zwraca prawde jeżeli usuwany bilet był na liście i usuną dany bilet
        /// /// </summary>
        /// <param name="UsuwanyBilet"></param>
        /// <returns></returns>
        public Boolean UsunBiletRezerwacje(RezerwcjaBilet UsuwanyBilet)
        {
            if (ListaBiletowRezerwacji.Count() != 0)
            {
                foreach (RezerwcjaBilet Obiekt in ListaBiletowRezerwacji)
                {
                    if (Obiekt == UsuwanyBilet)
                    {

                        ListaBiletowRezerwacji.Remove(UsuwanyBilet);
                        return true;
                    }             
                }
            }
            return false;
        }



    }
}
