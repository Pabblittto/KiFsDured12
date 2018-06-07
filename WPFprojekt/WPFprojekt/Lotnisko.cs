using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    /// <summary>
    /// Prosta klasa obiektu lotnisko , posiada tylko id, które jest jego nazwą
    /// </summary>
    [Serializable]
   public class Lotnisko
    {
        public string IDLotniska { get; set; }

        public Lotnisko(string ID)
        {
            IDLotniska = ID;
        }
        public string GetIDLotniska()
        {
            return IDLotniska;
        }
    }
}
