using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WPFprojekt
{
    
    public class Firma  
    {

        public DateTime WirtualnaData { get; set; }
        
        private TimeSpan Dodawanyczas = new TimeSpan(0,1,0);
        private DateTime Aktualnyczas;// czas potrzebny do sprawdzania czy minneła sekunda

        public String WirtualnaDataAktualzacja()
        {
            if(Aktualnyczas.Second!=DateTime.Now.Second)
            {
                Aktualnyczas = DateTime.Now;
                WirtualnaData=WirtualnaData.Add(Dodawanyczas);

            }
             return WirtualnaData.ToString();
        }


        public Firma()
        {
           // WirtualnaData = new DateTime(2018,8,12,12,8,0);//tą date trzeba wywalić tyz po piewrszych zapisach , odczytach , żeby była stała
            Aktualnyczas = DateTime.Now;
        }
        /// <summary>
        /// NOOOO panowie rozjebaneeeee , to działa tak , w typie wpisujesz czemu chcesz przydzielić ID to działa z trzeba klasami : Samolot, Lot i Klient, w Lista danych 
        /// Wpisujesz nazwe Listy danego typu czyli dla samolotu wpisujesz ListaSamolotow, a w LNIDdanych wpisujesz liste, która przechowuje id usunietych typów - to serio działa testowane
        /// </summary>
        /// <returns></returns>
        public string PrzydzielanieID<Typ>(List<Typ> ListaDanych, List<string> LNIDDanych) where Typ :KlasaID
        {
            if (LNIDDanych.Count() != 0)
            {
                string tmp = LNIDDanych[LNIDLotow.Count() - 1];
                LNIDDanych.Remove(tmp);
                return tmp;
            }
            else
            {
                if (ListaDanych.Count() != 0)
                {         
                    string NumerekBezformatu = ListaDanych[ListaDanych.Count() - 1].GetIDWlasne();// pobiera ID lotu który jest naj wcześniej - numerek jest ten w formacie 5 znaków jeżeli liczba nie zapełnia wszystkich 5 znaków to sa dodawane zera na początku       
                    
                    NumerekBezformatu = NumerekBezformatu.TrimStart('0');// usuwa zera z przodu
                    uint tmp = (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + NumerekBezformatu);// zamienia stringa na uinta 
                    tmp++;// zwiększa numerek przyszłego id
                    NumerekBezformatu = tmp.ToString("X5");
                    return NumerekBezformatu;
                }
                else
                    return "00001";// jeżeli na liście lotów nie ma żadnego lotu to pierwszy lot ma ID równe 00001- jest to liczba w szesnastkowym formacie
            }
        }// działa

        /// <summary>
        /// Funkcja która blokuje rezerwacje w lotach które mają się odbyć za 1 godz, automatyczna
        /// </summary>
        public void BlokujRezerwacje()
        {
            for (int i = 0; i < ListaLotow.Count(); i++)
            {
                ListaLotow[i].BlokujRezerwacje(WirtualnaData);
            }
        }
        

        /// <summary>
        /// Funkcja Wysyłająca w powietrze samoloty na podstawie nadanego czasu, automatyczna
        /// </summary>
        public void WyslijWKosmos()
        {
            for (int i = 0; i < ListaLotow.Count(); i++)
            {
                ListaLotow[i].WyslijWKosmos(WirtualnaData);
            }

        }

        public void UsunPlanLotu(PlanLotu Obiekt)
        {
            //funkcja co zwalnia pole samolotu czycykliczny z true/ ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }




        /// <summary>
        /// Usuwa Lot całkowicie- wywala z Listy lotów odbytych po 2 godzinach od landowania, funkcja automatyczna
        /// </summary>
        public void SprawdzenieStanuOdbytychLotow()
        {
            foreach (Lot Obiekt in ListaOdbytychLotow)
            {
                if(Aktualnyczas.CompareTo(Obiekt.DataLadowania.Add(new TimeSpan(2,0,0)))>=0)
                {
                    ListaOdbytychLotow.Remove(Obiekt);
                }
            }

        }
        /// <summary>
        /// Funkcja Przelatuje przez Liste lotów i tworzy nowy lot jeżeli ten leci w drują stonę wsadza odbyte loty do listy odbytych, automatycznie
        /// </summary>
        public void SprawdzanieStanuLotow()
        {
            foreach (Lot Obiekt in ListaLotow)
            {
                if(Obiekt.CzyWyladowal(Aktualnyczas)==true)
                {
                    if(Obiekt.CzyMaWracac==true)
                    {
                        Lot tmp = new Lot(Obiekt, PrzydzielanieID(ListaLotow, LNIDLotow),new TimeSpan(3,0,0));
                        DodawanieDoListy(ListaLotow, tmp);
                    }
                    ListaOdbytychLotow.Add(Obiekt);
                    ZwolnijSamolotZLOtu(Obiekt);
                    UsunZListy(ListaLotow, LNIDLotow,Obiekt);
                }
            }
        }




        /// <summary>
        /// Funkcja do porównywania ID dwóch objektów pojektów, jeżeli Objekt1 ma większe ID funkcja zwraca true
        /// </summary>
        /// <typeparam name="Typ"></typeparam>
        /// <param name="Objekt1"></param>
        /// <param name="Objekt2"></param>
        /// <returns></returns>
        public Boolean PorownanieID<Typ>(Typ Objekt1, Typ Objekt2) where Typ :KlasaID
        {
            uint temp1= (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + Objekt1.GetIDWlasne());
            uint temp2 = (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + Objekt2.GetIDWlasne());
            if (temp1 > temp2)
                return true;
            if (temp1 < temp2)
                return false;
            throw new Wyjatek("ID obiektów jest równe!! co nie powinno mieć miejsca!");// wyjątek, którego nie trzeba raczej obsługiwać, to info dla programisty, że pojawił się błąd logiczny
        }

        /// <summary>
        /// To specialna funkcja do dodawania do list, trzeba będzie z niej kożystać z powodu tego że funkcja PrzydzielanieId wymaga jakiegoś porządku na liście żeby dobrze działała
        /// To tyczy się wyłącznie: List: Samolotów
        /// </summary>
        public void DodawanieDoListy<Typ>(List<Typ> ListaDanych, Typ DodawanyObiekt) where Typ: KlasaID // później to napisze 
        {
            if (PorownanieID<Typ>(DodawanyObiekt, ListaDanych[ListaDanych.Count() - 1]))
                ListaDanych.Add(DodawanyObiekt);// jeżeli dodawany obiekt ma większe id jest dodawany na sam koniec
            else
                ListaDanych.Insert(0, DodawanyObiekt);
        }// trzeba to przetestować

        /// <summary>
        ///  Funkcja usuwająca z głównej listy dla klas: Samolot , Lot , Klient
        /// </summary>
        /// <typeparam name="Typ"></typeparam>
        /// <param name="ListaDanych"></param>
        /// <param name="LNIDDanych"></param>
        /// <param name="UsuwanyObiekt"></param>
        public void UsunZListy<Typ>(List<Typ> ListaDanych,List<string> LNIDDanych, Typ UsuwanyObiekt) where Typ: KlasaID
        {
            LNIDDanych.Add(UsuwanyObiekt.GetIDWlasne());
            ListaDanych.Remove(UsuwanyObiekt);
        }// trzeba to przetestować

        public void ZwolnijSamolotZLOtu(Lot Obiekt)
        {
            Obiekt.GetSamolot().CzyDostepny = true;
        }

        /// <summary>
        /// Funkcja zwraca true jeżeli Istnieje dane lotnisko z NazwaIDLotniska na liście
        /// </summary>
        /// <param name="NazwaIDLotniska"></param>
        /// <returns></returns>
        public Boolean CZyLotniskoIstnieje(string NazwaIDLotniska)
        {
            if (this.ListaLotnisk.Count() != 0)
            {
                foreach (Lotnisko Obiekt in this.ListaLotnisk)
                {
                    if (Obiekt.IDLotniska == NazwaIDLotniska)
                        return true;        
                }
                    return false;
            }
            else
               return false;
        }
        /// <summary>
        /// Funkcja do dodawania lotnisk do lity
        /// </summary>
        /// <param name="Dodawane"></param>
        public void DodajLotnisko(Lotnisko Dodawane)
        {
            this.ListaLotnisk.Add(Dodawane);
        }
         public void UsunLotniskoZOdpowiednimID(int NRid)
        {
            this.ListaLotnisk.Remove(ListaLotnisk[NRid]);
        }

        /// <summary>
        /// Zwraca true jeżeli Lotniska są identyczne
        /// </summary>
        /// <param name="Lotnis1"></param>
        /// <param name="Lotnis2"></param>
        public Boolean CzyLotniskaRozne(Lotnisko Lotnis1,Lotnisko Lotnis2 )
        {
            if (Lotnis1 == Lotnis2)
                return true;
            else
                return false;
        }

        /// <summary>
        ///Funkcja sprawdza czy nie trzeba dorobić lotów cylklicznych 
        /// </summary>
        public void AktualizacjaLotowCyklicznych()// fukcja jeszcze nie testowana
        {
            foreach (PlanLotu objekt in ListaPlanowLotu)
            {
                if (objekt.CzyTrzebaStworzyc(this.WirtualnaData))
                {
                    List<Lot> tmp = objekt.StworzLotyCykliczne(this.WirtualnaData);

                    foreach (Lot Polaczenie in tmp)
                    {
                        Polaczenie.SetID(PrzydzielanieID<Lot>(ListaLotow, LNIDLotow));// ustawienie ID ponieważ klasa Plan lotu tworzy Loty bez odpowiedniego ID
                                                                                      // trzeba tu napisać linijki do dodawania konkretnych samolotow do poszczególnych lotów, chyba że zrobimy to tak że tuż tuż przed lotem samolot jest dodawany!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        DodawanieDoListy<Lot>(ListaLotow, Polaczenie);
                    }
                }
            }
        }

        /// <summary>
        /// Funkcja dodająca do listy dwa obiekty Tras: Lotnisko1-Lotnisko2 oraz Lotnisko2-Lotnisko1
        /// </summary>
        /// <param name="Lotnisko1"></param>
        /// <param name=""></param>
        public void DodajTrase(Lotnisko Lotnisko1, Lotnisko Lotnisko2,int odleglosc)// można przetestować
        {
            Trasa tmp1 = new Trasa(Lotnisko1, Lotnisko2, odleglosc);
            Trasa tmp2 = new Trasa(tmp1);
            ListaTras.Add(tmp1);
            ListaTras.Add(tmp2);
        }


        // LNID to skrót : Lista Nieuzywanych ID jeżeli powstaje jakiś obiekt danego typu, a później jest on usuwany to program nie mógłby wykożystać jego id, bo nowe id tworzone przez funkcje, która dodaje 1 do wcześniejszego id, to sprawia że funkca nie może tworzyć wcześniejszych id!!
        public List<string> LNIDLotow = new List<string>();
        public List<string> LNIDKlientow = new List<string>();

        public List<Lot> ListaOdbytychLotow = new List<Lot>();// lista odbytych lotów bedzie trzymać obiekty przez następną godzinę lub dwie , później się wywali całkowicie
        public List<PlanLotu> ListaPlanowLotu = new List<PlanLotu>();
        public List<Lotnisko> ListaLotnisk=new List<Lotnisko>(); // zmienione na public żeby zrobić test
        public List<TypSamolotu> ListaTypow=new List<TypSamolotu>();
        public List<Trasa> ListaTras=new List<Trasa>();
        public List<Lot> ListaLotow=new List<Lot>();
        public List<Klient> ListaKlientow=new List<Klient>();
        //szybka notka jeszcze sprobuej ogarnac czy nie da sie jakos tych metod sprowadzic do takiej co by pobierala tylko parametry zeby nie bylo tego samego tysiac razy

         

        public void ZapisDoPliku()//generalnie zastosujemy serializacje zeby zapisywac calosc jeszcze doczytac musze czy to tak zadziala na jednym pliku ew jak to zrobic przy odczycie
        {
            try
            {
                using (Stream strumien = File.Open("dane.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(strumien, ListaLotnisk);
                    bin.Serialize(strumien, ListaTypow);
                    bin.Serialize(strumien, ListaTras);
                    bin.Serialize(strumien, ListaLotow);
                    bin.Serialize(strumien, ListaKlientow);
                    bin.Serialize(strumien, ListaTras);
                    bin.Serialize(strumien,LNIDKlientow);
                    bin.Serialize(strumien, LNIDLotow);
                    bin.Serialize(strumien, ListaPlanowLotu);
                    bin.Serialize(strumien, WirtualnaData);
                    bin.Serialize(strumien, ListaOdbytychLotow);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Wystapil blad zapisu danych");
            }
        } 
        public void OdczytZPliku()   // work in progress
        {
            using (Stream strumien = File.Open("dane.bin", FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();

                ListaLotnisk = (List<Lotnisko>) bin.Deserialize(strumien);
                ListaTypow = (List<TypSamolotu>) bin.Deserialize(strumien);
                ListaTras = (List<Trasa>) bin.Deserialize(strumien);
                ListaLotow = (List<Lot>) bin.Deserialize(strumien);
                ListaKlientow = (List<Klient>) bin.Deserialize(strumien);
                ListaTras = (List<Trasa>) bin.Deserialize(strumien);
                LNIDKlientow = (List<string>)bin.Deserialize(strumien);
                LNIDLotow = (List<string>)bin.Deserialize(strumien);
                ListaPlanowLotu = (List<PlanLotu>)bin.Deserialize(strumien);
                WirtualnaData = (DateTime)bin.Deserialize(strumien);
                ListaOdbytychLotow = (List<Lot>)bin.Deserialize(strumien);
           }
        }
 
    }
   
}
