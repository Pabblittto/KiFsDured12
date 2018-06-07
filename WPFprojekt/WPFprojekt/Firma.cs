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

        public DateTime WirtualnaData { get; set; }// będe tego później potrzebował, i w firmie bede pisał ewentualnie rózne funkcje odnośnie czasu~Pabblo
        
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
            WirtualnaData = new DateTime(2012,8,12,12,8,0);
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
        /// <param name="UsuwanyObjekt"></param>
        public void UsunZListy<Typ>(List<Typ> ListaDanych,List<string> LNIDDanych, Typ UsuwanyObjekt) where Typ: KlasaID
        {
            LNIDDanych.Add(UsuwanyObjekt.GetIDWlasne());
            ListaDanych.Remove(UsuwanyObjekt);
        }// trzeba to przetestować

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

        public List<PlanLotu> ListaPlanowLotu = new List<PlanLotu>();
        public List<Lotnisko> ListaLotnisk=new List<Lotnisko>(); // zmienione na public żeby zrobić test
        public List<TypSamolotu> ListaTypow=new List<TypSamolotu>();
        public List<Trasa> ListaTras=new List<Trasa>();
        public List<Lot> ListaLotow=new List<Lot>();
        public List<Klient> ListaKlientow=new List<Klient>();
        //szybka notka jeszcze sprobuej ogarnac czy nie da sie jakos tych metod sprowadzic do takiej co by pobierala tylko parametry zeby nie bylo tego samego tysiac razy
/*
        public Boolean DodajTyp(string Nazwa, int _Zasieg, int _Predkosc, int IlMiejsc, int IlMiejscVip)
        {
            if (ListaTypow.Count() != 0)
            {
                foreach (TypSamolotu Obiekt in ListaTypow)
                {
                    if (Obiekt.GetNazwaModelu() == Nazwa)
                        return false;
                }
            }
           
            ListaTypow.Add(new TypSamolotu(Nazwa,_Zasieg,_Predkosc,IlMiejsc,IlMiejscVip));
            return true;
        }
        public Boolean UsunTyp(string NazwaTypu)
        {
            if (ListaTypow.Count() != 0)
            {
                for (int i = 0; i < ListaTypow.Count(); i++)
                {
                    if (ListaTypow[i].GetNazwaModelu() == NazwaTypu)
                    {
                        ListaTypow.Remove(ListaTypow[i]);  //nie jestem pewien czy remove czy delete wiec zostanie na razie remove. jeszcze ogarne
                        return true;
                    }
                }
            }
            return false;
        }
        public void PrzegladTypow()
        {
            if (ListaTypow.Count() != 0)
            {
                for (int i = 0; i < ListaSamolotow.Count(); i++)
                {
                    //wypluwa kolejne typy na ekran
                }
            }
            else
            {
                //wypluwa komunikat o braku typow
            }
            
        }
        public Boolean DodajLotnisko(string id)
        {
            if (ListaLotnisk.Count() != 0)
            {
                foreach (Lotnisko Obiekt in ListaLotnisk)
                {
                    if (Obiekt.GetIDLotniska() == id)
                        return false;
                }
            }

            ListaLotnisk.Add(new Lotnisko(id));
            return true;
        }
        public Boolean UsunLotnisko(string id)
        {
            if (ListaLotnisk.Count() != 0)
            {
                for (int i = 0; i < ListaLotnisk.Count(); i++)
                {
                    if (ListaLotnisk[i].GetIDLotniska() == id)
                    {
                        ListaLotnisk.Remove(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public void PrzegladLotnisk()
        {
            if (ListaLotnisk.Count() != 0)
            {
                for (int i = 0; i < ListaLotnisk.Count(); i++)
                {
                    //wypluwa kolejne lotniska na ekran
                }
            }
            else
            {
                //wypluwa komunikat o braku lotnisk
            }

        }
        public Boolean DodajTrase(Lotnisko L1, Lotnisko L2,int odleglosc)
        {
            if (ListaTras.Count() != 0)
            { 
                foreach (Trasa Obiekt in ListaTras)
                {
                    int CzyIstnieje=0;
                    String[] RozbitaNazwa=Obiekt.GetIDTrasy.Split('-');// rozbija ID lotniska na podstawie jego tworzenia a potem sprawdza czy pomiedzy dwoma danymi lotniskami juz jest polaczenia
                       foreach(var Podciag in RozbitaNazwa)
                    {
                        if(String.Compare(Podciag,L1.GetIDLotniska,true)==0|| String.Compare(Podciag, L2.GetIDLotniska, true) == 0) // truew porownywaniu ignoruje wielkosc znakow
                        {
                            CzyIstnieje++;
                        }
                    }
                    if (CzyIstnieje == 2)
                        return false;
                }
            }

            ListaTras.Add(new Trasa(L1,L2,odleglosc));
            return true;
        }
        public Boolean UsunTrase(string id)
        {
            if (ListaLotnisk.Count() != 0)
            {
                for (int i = 0; i < ListaTras.Count(); i++)
                {
                    if (ListaTras[i].GetIDTrasy() == id)
                    {
                        ListaLotnisk.Remove(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public void PrzegladTras()
        {
            if (ListaTras.Count() != 0)
            {
                for (int i = 0; i < ListaTras.Count(); i++)
                {
                    //wypluwa kolejne trasy na ekran
                }
            }
            else
            {
                //wypluwa komunikat o braku tras
            }

        }
        public Boolean DodajKlienta(Klient nowy)//nie wiem jeszcze jak rozwiazac dodawanie osoby lub posrednika. albo bedzie wybor opcji np przez parametr albo bedzie tworzony oddzielnie a potem dodany na liste
        {
            if (ListaKlientow.Count() != 0)
            {
                foreach (Klient Obiekt in ListaLotnisk)
                {
                    if (Klient.GetIDKlienta() == id)
                        return false;
                }
            }

            ListaLotnisk.Add(nowy);
            return true;
        }
        public Boolean UsunKlienta(string id)
        {
            if (ListaKlientow.Count() != 0)
            {
                for (int i = 0; i < ListaKlientow.Count(); i++)
                {
                    if (ListaKlientow[i].GetIDKlienta() == id)
                    {
                        ListaLotnisk.Remove(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public void PrzegladKlientow()
        {
            if (ListaKlientow.Count() != 0)
            {
                for (int i = 0; i < ListaKlientow.Count(); i++)
                {
                    //wypluwa kolejne lotniska na ekran
                }
            }
            else
            {
                //wypluwa komunikat o braku lotnisk
            }

        }
        */
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

            }
        }
 
    }
   
}
