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

        public Boolean CzyDostepny { get; set; }
        public Lot CoObsluguje { get; set; }
        public Boolean Cykliczny { get; set; }// zmienna potrzebna dla lotów cyklicznych 

        public Samolot(String ID)
        {
            SetID(ID);
            CzyDostepny = true;
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
