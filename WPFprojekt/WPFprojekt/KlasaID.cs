using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    public abstract class KlasaID
    {
       protected string IDObiektu;

        public string GetIDWlasne()
        {
            return IDObiektu;
        }

        public void SetID(string a)
        {
            IDObiektu = a;
        }

    }
}
