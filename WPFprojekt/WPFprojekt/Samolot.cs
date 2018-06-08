using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    public class Samolot : KlasaID
    {
       // private string IDSamolotu;

        public Boolean CzyDostepny { get; set; }// zmienna dla pojedynczych lotów
        public Lot CoObsluguje { get; set; }
        public Lot Coobsluguje2;// zmienna dla lotów które wracają i muszą trzymać lot powrotny
        public PlanLotu PlanLotuPrzypisany { get; set; }
        public Boolean Cykliczny { get; set; }// zmienna działa tak jak czy dostepny , ale Dla lotów cykliczbych

        public Samolot(String ID)
        {
            SetID(ID);
            CzyDostepny = true;
        }

        public Samolot()
        {
            CzyDostepny = true;
            CoObsluguje = null;
            Coobsluguje2 = null;
            PlanLotuPrzypisany = null;
        }
        /// <summary>
        /// Funkcja prosta i może sie przydać zarówno do automatycznego wysyłania samolotów w powietrze jak 
        /// i do sprowadzania samolotów na ziemie, zmienia stan CzyDsotepny na przeciwny, ewentualnie wywalić
        /// </summary>
        public void ZmianaDostepu()// to trzeba ewentualnie zmienic
        {
            CzyDostepny = !CzyDostepny;
        }

        public void Zeruj()
        {
            CoObsluguje = null;
            Coobsluguje2 = null;
            CzyDostepny = true;
            Cykliczny = false;
        }

        public Boolean CzyMaNastepnylot()
        {
            if (Coobsluguje2 != null)
                return true;
            else
                return false;
        }
        public void PrzepiszLoty()
        {
            CoObsluguje = Coobsluguje2;
            Coobsluguje2 = null;
        }

    }
}
