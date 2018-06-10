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
            if (Aktualnyczas.Second != DateTime.Now.Second)
            {
                Aktualnyczas = DateTime.Now;
                WirtualnaData = WirtualnaData.Add(Dodawanyczas);

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
                    string NumerekBezformatu = ListaDanych[ListaDanych.Count() - 1].IDObiektu;// pobiera ID lotu który jest naj wcześniej - numerek jest ten w formacie 5 znaków jeżeli liczba nie zapełnia wszystkich 5 znaków to sa dodawane zera na początku       
                    
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


        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void PrzyciskUsunSamolot(Samolot Wskazany, TypSamolotu JakiTyp)
        {
            if (Wskazany != null && JakiTyp != null)
            {
                if (Wskazany.CzyDostepny == true && Wskazany.Cykliczny == false)
                {
                    JakiTyp.GetListaSamolotow().Remove(Wskazany);
                }
                else throw new Wyjatek("Samolot obsługuje aktualnie jakis lot");
            }
            else throw new Wyjatek("Wybierz dwa obiekty");
        }

         public void PrzyciskUsunTypSamolotu(TypSamolotu Wskazany)
        {
            if (Wskazany.GetListaSamolotow().Count() == 0)
            {
                ListaTypow.Remove(Wskazany);
            }
            else throw new Wyjatek("Istnieją jeszcze samoloty tego typu, najpierw je usun");
        }

        public void PrzyciskAnulujRezerwacje(RezerwcjaBilet Dowywalenia)
        {
            if (Dowywalenia != null)
            {
                Dowywalenia.Polaczenie.AnulujRezerwacje(Dowywalenia.Pasazer, Dowywalenia);
            }
            else
                throw new Wyjatek("Zaznacz konkretny obiekt!");
        }

        public void PrzyciskUsunKlienta(Klient Wskazany)
        {
            if (Wskazany != null)
            {
                UsunKlienta(Wskazany);
            }
            else
                throw new Wyjatek("Musisz wybrać klienta");
        }

        public void PrzyciskUsunPlanLotu(PlanLotu Wskazany)
        {
            if (Wskazany != null)
            {
                Wskazany.Pojazd.Zeruj();
                ListaPlanowLotu.Remove(Wskazany);
            }
            else throw new Wyjatek("Wybierz Plan Lotu do usunięcia");
        }
         
        public void PrzyciskUsunLot(Lot WskazanyLot)
        {
            if (WskazanyLot != null)
            {
                if (WskazanyLot.Maszyna.CoObsluguje == WskazanyLot  )
                {
                    if (WskazanyLot.Maszyna.CzyMaNastepnylot() == true)
                    {
                        WskazanyLot.Maszyna.PrzepiszLoty();
                    }
                    else WskazanyLot.Maszyna.Zeruj();
                }
                else WskazanyLot.Maszyna.Coobsluguje2 = null;

               UsunZListy(ListaLotow, LNIDLotow, WskazanyLot);
            }
            else throw new Wyjatek("Wybierz Lot z listy");
        }

        public void PrzyciskUsunLotnisko(Lotnisko Wskazane )
        {
            if (Wskazane != null)
            {
                foreach (Trasa Obiekt in ListaTras)
                {
                    if (Obiekt.Lotnisko1 == Wskazane || Obiekt.Lotnisko2 == Wskazane)
                        throw new Wyjatek("Istnieją Trasy zawierające te lotnisko, Usun te trasy");
                }
                ListaLotnisk.Remove(Wskazane);

            }
            else
                throw new Wyjatek("Wybierz obiket z listy");
        }

        /// <summary>
        /// Usunięcie trzasy wiąże się z usunięciem trasy odwróconej!!!
        /// </summary>
        /// <param name="Wskazana"></param>
        public void PrzyciskUsunTrase(Trasa Wskazana)////////////////////////////////////////////// Nie wiem czy zadzaiła!!!!!!!
        {
            if (Wskazana != null)
            {
                foreach (Lot Obiekt in ListaLotow)
                {
                    if (Obiekt.Droga == Wskazana || Obiekt.Droga == new Trasa(Wskazana))// to można uprościc prawdopodobnie do tylko wskazania , bo trase w drugą stronę samoloty robią sobie same
                        throw new Wyjatek("Istnieją loty które mają tą trasę , lub trasę odwróconą");
                }
                ListaTras.Remove(Wskazana);
                ListaTras.Remove(new Trasa(Wskazana));// gdyby to nie działało to można usunąć tylko jedną trasę , Każdy samolot otrzymuje w konstruktorze nowy obiekt odwróconej trasy
            }
            else
                throw new Wyjatek("Wybierz obiekt z listy");
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
            /// Funkcja wywala dwa wyjątki: Wypełnienie wszystkich pól i brak miejsc w samolocie
            /// </summary>
        public void PrzyciskRezerwuj(Lot JakiLot, Klient JakiKlient,Boolean Czywykupil,Boolean CzyVIP)
        {
            if (JakiLot!=null && JakiKlient!=null)
            {
                if (JakiLot.CzyzablokowaneBookowanie == false)
                {
                    JakiLot.RezerwujKupBilet(JakiKlient, CzyVIP, Czywykupil, this.WirtualnaData);
                }
                else
                    throw new Wyjatek("Do Odlotu zostało mniej niż 1 godz, nie można rezerwować biletów");

            }
            else
                throw new Wyjatek("Wypełnij wszystkie pola!!");
        }

        public void PrzyciskDodajKlientaPosrednika(string nazwa)
        {
            Posrednik tmp = new Posrednik(nazwa, PrzydzielanieID(ListaKlientow, LNIDKlientow));
            DodawanieDoListy(ListaKlientow, tmp);
        }

        /// <summary>
            /// Funkcja nie zwraca żdnych wyjatków
            /// </summary>
        public void PrzyciskDodajKlientaOsobe(string imie, string nazwisko)
         {
            Osoba tmp = new Osoba(imie, nazwisko, PrzydzielanieID(ListaKlientow, LNIDKlientow));
            DodawanieDoListy(ListaKlientow,tmp);
         }

        /// <summary>
        /// Index to numer TypuSamolotu na liśćie - to ma określić do jakiego typu ma dodać Samoloty
        /// </summary>
        public void PrzyciskDodajModelSamolotu(int index, int ilosc)
        {
            for(int i=1;i<=ilosc; i++)
                {
                ListaTypow[index].DodajSamolot(PrzydzielanieID(ListaTypow[index].GetListaSamolotow(), ListaTypow[index].GetLNIDSamolotow()));
                }
        }

        /// <summary>
        ///  Funkcja wywala 
        /// </summary>
        public void PrzyciskDodajLot(Trasa Droga, DateTime DataWylotu,Boolean CzyMawracac,TypSamolotu _WybranytypSamolotu,Samolot WybranaMaszyna)
        {
            if (Droga != null && DataWylotu != null && _WybranytypSamolotu != null && WybranaMaszyna!=null)
            {
                if (_WybranytypSamolotu.GetZasieg() >= Droga.Odleglosc)
                {
                    Lot tmp = new Lot(PrzydzielanieID(ListaLotow, LNIDLotow), Droga, DataWylotu, CzyMawracac, WybranaMaszyna, _WybranytypSamolotu);
                    if(CzyMawracac==true)
                    {
                        Lot tmp2 = new Lot(PrzydzielanieID(ListaLotow,LNIDLotow),Droga,DataWylotu.Add(tmp.CzasLotu.Add(new TimeSpan(3,0,0))),CzyMawracac,WybranaMaszyna,_WybranytypSamolotu);
                        DodawanieDoListy(ListaLotow, tmp2);
                    }
                    DodawanieDoListy(ListaLotow, tmp);
                }
                else
                    throw new Wyjatek("Samolot nie doleci do celu, zmień jego rodzaj!");
            }
            else
                throw new Wyjatek("Wypełnij wszystkie pola!");
        }   

        /// <summary>
        /// Funkcja do dodawanie PlanuLotu, zwraca 3 wyjątki: albo nie wszystkie pola pełne , albo że zdąży wrócić przed nsateonym lotem, że nie
        /// dolec ponieważ typ samolotu ma za mały sasięg , lub też że nie zdąży wrócić przed następnym lotem
        /// </summary>
        public void PrzyciskDodajPlanLotu(DateTime _PierwszyLot, TimeSpan _CoIlelata,Trasa Kierunek,TypSamolotu _RodzajSamolotu,TimeSpan NaJakiPrzedzialCzasu,Samolot _PojazdPermamentny)
        {
            if (_PierwszyLot != null && _CoIlelata != null && Kierunek != null && _RodzajSamolotu != null && NaJakiPrzedzialCzasu != null && _PojazdPermamentny != null)
            {
                if (PlanLotu.CzyDoleci(_RodzajSamolotu,Kierunek) == true)
                {
                    if (PlanLotu.CzyzdarzyWrocic(_RodzajSamolotu, Kierunek, _CoIlelata) == true)
                    {
                        ListaPlanowLotu.Add(new PlanLotu(_PierwszyLot, _CoIlelata, Kierunek, _RodzajSamolotu, NaJakiPrzedzialCzasu, _PojazdPermamentny));
                    }
                    else
                        throw new Wyjatek("Samolot nie zdąży wrócić do Lotniska bazoewgo");
                }
                else
                    throw new Wyjatek("Samolot ma za mały zasięg");
            }
            else
               throw new Wyjatek("Wypełnij wszystkie pola!");
        }

        /// <summary>
        /// Wywala wyjatek jeżeli wybierzesz takie same lotniska
        /// </summary>
        public void PrzyciskDodajTrase(Lotnisko Lot1,Lotnisko Lot2,uint odl)
        {
            if (Firma.CzyLotniskaRozne(Lot1, Lot2) == true || odl>0)
            {
                foreach (Trasa Obiekt in ListaTras)
                {
                    if ((Obiekt.Lotnisko1==Lot1 && Obiekt.Lotnisko2==Lot2) ||(Obiekt.Lotnisko1==Lot2 && Obiekt.Lotnisko2==Lot1))
                        throw new Wyjatek("Istnieje już taka trasa!");
                }
                this.DodajTrase(Lot1, Lot2,(int)odl);
            }
            else
                throw new Wyjatek("Wybrałeś takie same lotniska!");
        }

        /// <summary>
        /// Funkcja wywala 2 wyjatki- jak nie pwrowadzi sie nazwy lotniska lub jak Istnieje juz takie lotnisko na liście 
        /// </summary>
        /// <param name="Nazwa"></param>
        public void PrzyciskDodajLotnisko(string Nazwa)
        {
            if (Nazwa != "" || CZyLotniskoIstnieje(Nazwa)==false)
            {
                foreach (Lotnisko Obiekt in ListaLotnisk)
                {
                    if (Obiekt.IDLotniska.ToLower() == Nazwa.ToLower())
                        throw new Wyjatek("Istnieje już takie lotnisko");
                }
                    this.ListaLotnisk.Add(new Lotnisko(Nazwa));
            }
            else
                throw new Wyjatek("Wprowadź inną nazwe lotniska");
        }

        public void PrzyciskDodajTypSamolotow(string Nazwa,string Zasieg, string Predkosc,string Iloscmiejsc, string IloscMiejscVIP)
        {
            int x=0, y=0, z=0, w=0;
            if(Int32.TryParse(Zasieg,out x)==true || Int32.TryParse(Predkosc, out y) == true || Int32.TryParse(Iloscmiejsc, out z) == true || Int32.TryParse(IloscMiejscVIP, out w) == true )
            {
                if (Nazwa != "" && x > 0 && y > 0 && z > 0 && w > 0)
                {
                    ListaTypow.Add(new TypSamolotu(Nazwa, x, y, z, w));
                }
                else throw new Wyjatek("Błąd wprowadzonych danych!!Sprawdz je");
            }
            throw new Wyjatek("Wprowadz odpowiednią liczbe w odpowiednich miejscach");
        }

        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        /// Funkcja Przelatuje przez Liste lotów i tworzy nowy lot jeżeli ten leci w drują stonę, wsadza odbyte loty do listy odbytych, automatycznie
        /// </summary>
        public void SprawdzanieStanuLotow()
        {
            foreach (Lot Obiekt in ListaLotow)
            {
                if(Obiekt.CzyWyladowal(Aktualnyczas)==true)
                {
                    ListaOdbytychLotow.Add(Obiekt);
                    if (Obiekt.Maszyna.Cykliczny == false && Obiekt.Maszyna.Coobsluguje2==null) Obiekt.Maszyna.Zeruj();
                    if (Obiekt.Maszyna.Cykliczny == false && Obiekt.Maszyna.CzyMaNastepnylot() == true) Obiekt.Maszyna.PrzepiszLoty();
                    UsunZListy(ListaLotow, LNIDLotow,Obiekt);
                }
            }
        }


        public void PrzedawnianieRezerwacji()
        {
            foreach (Lot Obiekt in ListaLotow)
            {
                Obiekt.PrzedawniajRezerwacje(WirtualnaData);
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
            uint temp1= (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + Objekt1.IDObiektu);
            uint temp2 = (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + Objekt2.IDObiektu);
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
            LNIDDanych.Add(UsuwanyObiekt.IDObiektu);
            ListaDanych.Remove(UsuwanyObiekt);
        }// trzeba to przetestować


        /// <summary>
        /// Funkcja zwraca true jeżeli Istnieje dane lotnisko z NazwaIDLotniska na liście
        /// </summary>
        /// <param name="NazwaIDLotniska"></param>
        /// <returns></returns>
        public  Boolean CZyLotniskoIstnieje(string NazwaIDLotniska)
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

         public void UsunLotniskoZOdpowiednimID(int NRid)
        {
            this.ListaLotnisk.Remove(ListaLotnisk[NRid]);
        }

        /// <summary>
        /// Zwraca true jeżeli Lotniska są identyczne
        /// </summary>
        /// <param name="Lotnis1"></param>
        /// <param name="Lotnis2"></param>
        public static Boolean CzyLotniskaRozne(Lotnisko Lotnis1, Lotnisko Lotnis2 )
        {
            if (Lotnis1==Lotnis2)
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

        private void UsunKlienta(Klient Ktokolwiek)
        {
            foreach (RezerwcjaBilet Bilecik in Ktokolwiek.GetListaBiletowRezerwacji())
            {
                Bilecik.Polaczenie.AnulujRezerwacje(Ktokolwiek, Bilecik);
            }
            UsunZListy(ListaKlientow, LNIDKlientow, Ktokolwiek);
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
