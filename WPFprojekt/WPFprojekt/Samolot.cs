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

        private Boolean CzyDostepny;

        public Samolot(String ID)
        {
            SetID(ID);
            CzyDostepny = true;
        }

        private Lot CoObsluguje;// może opłacało by się to napisać ????????????????????????????????????????????????????????????????????????????????????????????????????????????????? nie dokonczone
                                // przyda sie przy wyświetlaniu samolotow- albo jest wolny albo wypisuje połączenie



        public Boolean GetCzyDostepny()
        {
            return CzyDostepny;
        }

      /*  public void SetCzyDostepny(Boolean stan)
        {
            CzyDostepny = stan;
        }
        */

        /// <summary>
        /// Funkcja prosta i może sie przydać zarówno do automatycznego wysyłania samolotów w powietrze jak 
        /// i do sprowadzania samolotów na ziemie, zmienia stan CzyDsotepny na przeciwny, ewentualnie wywalić
        /// </summary>
        public void ZmianaDostepu()
        {
            CzyDostepny = !CzyDostepny;
        }


    }
}
