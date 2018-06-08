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
       public string IDObiektu { get; set; }

        public void SetID(string a)
        {
            IDObiektu = a;
        }

    }
}
