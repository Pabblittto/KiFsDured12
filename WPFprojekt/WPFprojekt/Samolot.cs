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


    }
}
