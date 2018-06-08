using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFprojekt
{
    [Serializable]
    public class Lot : KlasaID
    {
         public string IDLotu { get; set; }
        public string IDSamolotu { get; set; }// id konkretnego zamolotu, który obsługuje lot 

        private List<RezerwcjaBilet> ListaRezerwacji; // lista rezerwacji określa również liste/liczbe klientów lecących tym samolotem
         private List<string> LNIDRezerwacjiBiletow;

        public Samolot Maszyna { get;set; }
        public Trasa Droga { get; set; }
        public TypSamolotu Pojazd { get; set; }// typ samolotu, ponieważ on przechowuje prekość, ładowność itd.
        public TimeSpan CzasLotu { get; set; } //  ten czas jest liczony i wklepywany przez funkcje
        public DateTime DataGodzinaWylotu { get; set; }
        public DateTime DataLadowania { get; set; }

       // public Boolean CzyLotNalezyDoCyklicznych;
        public Boolean CzyMaWracac;// zmienna określająca czy ma wrocić, koncepcja lotu polega na tym że leci do miejsca docelowego , a później wraca, tworzy to dwa połączenia , można w sumie wywalić i trzeba określać loty w dwie strony oddzielnie
        public Boolean CzyWlocie = false;
        public Boolean CzyzablokowaneBookowanie = false;

        /// <summary>
        /// Podstawowy konstruktor do lotu pojedynczego
        /// </summary>
        public Lot(string ID, Trasa _Droga, DateTime DataWylotu,Boolean _CZyMawracac, Samolot MaszynaObslugujaca,TypSamolotu RodzajSamolotu)
        {
            Pojazd = RodzajSamolotu;
            Maszyna = MaszynaObslugujaca;
            LNIDRezerwacjiBiletow = new List<string>();
            ListaRezerwacji = new List<RezerwcjaBilet>();
            SetID(ID);
            Droga = _Droga;
            DataGodzinaWylotu = DataWylotu;//ostatnia liczna to sekundy- nieistotna wartość w programie
            Pojazd = null;// to też pomaga stwierdzić czy istnieje samolot który jest zapisany do trasy
            CzasLotu = new TimeSpan(0, 0, 0);//dzięki temu wiemy że na początku nie ma konkretnego samolotu który obsługuje ta trase
            CzyMaWracac = _CZyMawracac;
            Maszyna.Cykliczny = false;
            Maszyna.CzyDostepny = false;
            CzasLotu = PlanLotu.ILeleciWjednaStrone(RodzajSamolotu, _Droga);
            DataLadowania= this.DataLądowaniaDateTime();

            if(CzyMaWracac==true && Maszyna.CoObsluguje!=null)
            {
                Maszyna.Coobsluguje2 = this;
            }
            else
            Maszyna.CoObsluguje = this;
        }
        /// <summary>
        /// Konstruktor dla lotów cyklicznych
        /// </summary>
        public Lot(string ID, Trasa Droga, DateTime Datawylotu,TypSamolotu TypPrzypisanyDoPlanu, Samolot Maszyna,PlanLotu JakiPlan):this(ID,Droga,Datawylotu,true, Maszyna,TypPrzypisanyDoPlanu)
        {
            Maszyna.Cykliczny = true;
            Maszyna.CoObsluguje = null;
            Maszyna.PlanLotuPrzypisany = JakiPlan;

        }

        /// <summary>
        /// Specialny konstruktor Lotu, zakłada powrót tego samego samolotu więc tworzony lot ma wszystko takie same prócz: IDLotu , daty wylotu, kolejności Lotnisk w trasie, po tym wywołaniu stary lot powinien zostac usunięty
        /// </summary>
        //public Lot(Lot IstniejacyLOt,string _IDLotu,TimeSpan IloscCzasuDoStartuLiczonaOdMomentuLondowania)
        //{
        //    this.LNIDRezerwacjiBiletow = new List<string>();
        //    this.ListaRezerwacji = new List<RezerwcjaBilet>();

        //    IstniejacyLOt.GetSamolot().CzyDostepny=true;// taki cheat żeby przez chwile samolt był dostępny ten cheat się komplikuje wiestety , lepiej nie ruszać
        //    this.SetPojazd(IstniejacyLOt.Pojazd);
        //    this.SetIDSamolotuWLocie(IstniejacyLOt.GetSamolot().GetIDWlasne());
        //    IstniejacyLOt.GetSamolot().CzyDostepny=false;// taki cheat 
        //    this.DataGodzinaWylotu = IstniejacyLOt.DataLądowaniaDateTime().Add(IloscCzasuDoStartuLiczonaOdMomentuLondowania);
        //    this.CzasLotu = IstniejacyLOt.GetCzasLotu();
        //    this.Droga = new Trasa(IstniejacyLOt.GetDroga());
        //    CzyMaWracac = false;
        //}


        /// <summary>
        ///  Z racji że typ samolotu ma dany zasięg, trzeba sprawdazć czy dany samolot nadaje się do lotu 
        ///  dla konkretnej trasy, podawy jest od razu id samolotu, bardzo ważna funkcja, bez niej lot nie mam maszyny!!
        ///  funkcja od razu liczy nowy czas przelotu danej trasy
        /// </summary>
        /// <returns></returns>
        public Boolean SetPojazd(TypSamolotu TypPojazdu)//  nie jest w konstruktorze ponieważ zwraca booleana
        {
            if (TypPojazdu.GetZasieg() >= Droga.GetOdleglosc())
            {
                Pojazd = TypPojazdu;
                double czas = Droga.GetOdleglosc() / Pojazd.GetPredkosc();// czas wychodzi w godz z minutamie po przecinku
                czas = Math.Round(czas, 2);
                double min = (czas % 1) * 60;// minuty w formiacie 0,xx więc trzeba pomnożyć razy 60
                CzasLotu = new TimeSpan((int)czas,(int)min,0);// zero na końcu- to sekundy nieistotne w programie
                DataLadowania = this.DataLądowaniaDateTime();
                return true;
            }
            else
                return false;
        }



        /// <summary>
        /// Ustawienie Samolotu dla danego lotu , zwraca true jeżlei ustawienie sie udało , zwraca false jeżeli samolot jest niedostępby lub jeżeli jest cykliczny
        /// Funkcja zmienia dostępnośc samolotu na false
        /// </summary>
        /// <param name="IDPojazdu"></param>
        /// <returns></returns>

        /// <summary>
        ///Funkcja wysyłająca w kosmos jeżeli przyjdzie na to czas
        /// </summary>
        public void WyslijWKosmos(DateTime AktualnaData)
        {
            if (CzyWlocie == false)
            {
                if (AktualnaData.CompareTo(DataGodzinaWylotu) >= 0)
                {
                    CzyWlocie = true;
                }
            }
        }


        public int GetIloscMiejscWolnychZwyklych()
        {
            if (Pojazd != null)
            {
                if (ListaRezerwacji.Count() != 0)
                {
                    int liczba = 0;
                    foreach (RezerwcjaBilet Objekt in ListaRezerwacji)
                    {
                        if (Objekt.CzyVIP() == false)
                            liczba++;
                    }

                    return Pojazd.GetIloscMiejsc() - liczba;
                }
                else
                    return Pojazd.GetIloscMiejsc();
            }
            else
                throw new Wyjatek("Nie został dodany samolot obsłudujący ten lot, lub został!!");// niech uzytkownik wypełni pole samolotu!! niech w ekstrymalnych przypadkach program usuwa                                                                           // dany lot żebby problemy się nie robiły
        }


        public int GetIloscMiejscWolnychVip()
        {
            if (Pojazd != null)
            {
                if (ListaRezerwacji.Count() != 0)
                {
                    int liczba = 0;
                    foreach (RezerwcjaBilet Objekt in ListaRezerwacji)
                    {
                        if (Objekt.CzyVIP() == true)
                            liczba++;
                    }

                    return Pojazd.GetIloscMiejscVip() - liczba;
                }
                else
                    return Pojazd.GetIloscMiejscVip();
            }
            else
                throw new Wyjatek("Nie został dodany samolot obsłudujący ten lot, lub został on usunięty!!");   
        }


        /// <summary>
        /// Data wylotu podana w Stringu
        /// </summary>
        /// <returns></returns>
        public string GetDataWylS() 
        {
            return DataGodzinaWylotu.ToString();
        }

        /// <summary>
        /// Data wylotu podana w DateTime
        /// </summary>
        /// <returns></returns>
        public DateTime GetDataWylDT()
        {
            return DataGodzinaWylotu;
        }

        /// <summary>
        /// zwraca obiekt samolotu, który obsługuje trase- może się przydać
        /// </summary>
        /// <returns></returns>
        //public Samolot GetSamolot() 
        //{
        //    if (Pojazd != null)
        //    {
        //        foreach (Samolot Obiekt in Pojazd.GetListaSamolotow())
        //        {
        //            if (Obiekt.GetIDWlasne() == IDSamolotu)
        //                return Obiekt;
        //        }
        //    }
        //    // to miejsce gdzie pojazd nie jest równy null , ale nie ma jego obiektu na liście
        //    Pojazd = null;
        //    throw new Wyjatek("Nie ma Samolotu na liście typów !!");// bardzo specyficzny wyjątek , ktoś usuną samolot, który obsługiwał tą trasę co powinno być nie możliwe-
        //                                                            // w catchu proponuje napisać krótką funkcję zmieniającą pole "Pojazd" na null!!!-Ważne
        //}

            public Samolot GetSamolot()
            {
                return Maszyna;
            }


        /// <summary>
        /// Funkcja zrwaca true jeżlei lot wylądował i mozna go usunąć
        /// </summary>
        /// <param name="Aktualnyczas"></param>
        /// <returns></returns>
        public Boolean CzyWyladowal(DateTime Aktualnyczas)
        {
            if (CzyzablokowaneBookowanie == true && CzyWlocie == true && Pojazd!=null)
            {
                if (Aktualnyczas.CompareTo(this.DataLadowania) >= 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// DAta lądowania liczona na podstawie czasu lotu podawana w Stringu
        /// </summary>
        /// <returns></returns>
        public String DataLądowaniaString() 
        {
            if (Pojazd != null)
            {
                DateTime cont = DataGodzinaWylotu.Add(CzasLotu);
                return cont.ToString();
            }
            else
                throw new Wyjatek("Nie został dodany samolot obsłudujący ten lot!!");// w cathu jakiś komunikat dla uzytkownika żeby dodał samolot-Wazne
        }

        /// <summary>
        /// Data Lądowania podawana w Date time
        /// </summary>
        /// <returns></returns>
        public DateTime DataLądowaniaDateTime()
        {
            if (Pojazd != null)
            {
                DateTime cont = DataGodzinaWylotu.Add(CzasLotu);
                return cont;
            }
            else
                throw new Wyjatek("Nie został dodany samolot obsłudujący ten lot!!");// w cathu jakiś komunikat dla uzytkownika żeby dodał samolot-Wazne
        }


        /// <summary>
        /// Funkcja przydzialająca ID nowym rezerwacjom , robi to na podstawie ostatniej rezerwacji dodanej do listy lub bierze je z "kosza"
        /// </summary>
        /// <returns></returns>
        public string PrzydzielanieIDRezerwacja()
        {
            if(LNIDRezerwacjiBiletow.Count()!=0)
            {
                string tmp = LNIDRezerwacjiBiletow[LNIDRezerwacjiBiletow.Count() - 1];
                LNIDRezerwacjiBiletow.Remove(tmp);
                return tmp;
            }
            else
            {
                if (ListaRezerwacji.Count()!=0)
                {
                    string NumerekbezFormatu = ListaRezerwacji[ListaRezerwacji.Count() - 1].GetNrRezerwacji();
                    NumerekbezFormatu = NumerekbezFormatu.Split('-')[1];
                    uint tmp = (uint)new System.ComponentModel.UInt32Converter().ConvertFromString("0x" + NumerekbezFormatu);
                    tmp++;
                    NumerekbezFormatu = tmp.ToString("X3");
                    return IDObiektu + "-" + NumerekbezFormatu;
                }
                else
                    return IDObiektu +"-"+"001";

            }

        }

        /// <summary>
        /// Funkcja potrzebna w sytuacji, kiedy Id jest brane z listy usuniętych obiektów , ta funkcja sprawdza czy Taka sytuacja miała miejsce
        /// Zwraca true jeżeli id pierwszego biletu jest większa, zwraca false w przeciwnym wypadku
        /// </summary>
        /// <returns></returns>
        public Boolean PorownajIDBiletow(RezerwcjaBilet Bilet1,RezerwcjaBilet Bilet2)
        {
            if(Bilet1!=null && Bilet2!=null)
            {
                if (Bilet1.NrMiesca > Bilet2.NrMiesca)
                return true;

            if (Bilet1.NrMiesca < Bilet2.NrMiesca)
                return false;

            throw new Wyjatek("Bardzo poważny problem , dwa bilety mają ten sam numer miejsca co nie powinno mić miejsca!");// poważny błąd, raczej nie trzeba go obsługiwac , napisany po to żeby 
                                                                                                                            // zawiadomic że wystąpił błąd logiczny i trzeba popatrzeć w kodzik- możliwe że jakieś 
                                                                                                                            //id z kosza zostało dane na koniec listy i tworzą się kopie id które juz jest na liści
            }
            throw new Wyjatek("Jeden z obiektów jest nullem");
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AktualnaData"></param>
        public void BlokujRezerwacje(DateTime AktualnaData)
        {
            if(CzyzablokowaneBookowanie==false)
            {
            if (AktualnaData.CompareTo(DataGodzinaWylotu.Subtract(new TimeSpan(1, 0, 0))) >=0)
                    {
                    CzyzablokowaneBookowanie = true;
                    }
            }
        }

        public void PrzedawniajRezerwacje(DateTime Aktualnadata)
        {
            foreach (RezerwcjaBilet Bilecik in ListaRezerwacji)
            {
                if(Bilecik.CzyKupionyBilet==false && Bilecik.CzyWygaslo(Aktualnadata)==true)
                {
                    this.AnulujRezerwacje(Bilecik.Pasazer, Bilecik);
                }
            }
        }

        /// <summary>
        /// Funkcja dodająca rezerwacje na dany lot dla danej osoby, CyToKupno określa czy klient rezerwuje czy kupuje od razu bilet
        /// </summary>
        /// <param name="_Klient"></param>
        /// <param name="CzyVIP"></param>
        /// <returns></returns>
        public void RezerwujKupBilet(Klient _Klient, Boolean CzyVIP, Boolean CzyToKupno, DateTime AktualnaData)
        {
            if ((this.GetIloscMiejscWolnychZwyklych() != 0 && CzyVIP == false) || (this.GetIloscMiejscWolnychVip() != 0 && CzyVIP == true))// funkcja sprawdza czy można zarezerwować miejsce
            {
                RezerwcjaBilet NowyBilecikRezerwacja = new RezerwcjaBilet(PrzydzielanieIDRezerwacja(), 0, CzyVIP, _Klient, AktualnaData, CzyToKupno, this);// te zero trzeba zamienic na funkcje liczącą cene biletu , na przykład na podstawie odległości
                _Klient.DodajBiletRezerwacje(NowyBilecikRezerwacja);

                if (PorownajIDBiletow(NowyBilecikRezerwacja, ListaRezerwacji[ListaRezerwacji.Count() - 1]))// to sytuacja kiedy nowy obiekt ma większe id - jest dodawany na koncu
                    ListaRezerwacji.Add(NowyBilecikRezerwacja);
                else
                    ListaRezerwacji.Insert(0, NowyBilecikRezerwacja);
            }
            else
                throw new Wyjatek("Brak miejsc wolnych");
            
        }

        /// <summary>
        ///  Funkcja usuwająca rezerwacje i bilety- usuwa z listy w kliencie i z listy w Locie
        ///  Nie ma żadanego boola czy zwracania wyjątków , więc z góry zakładamy że biletdousunięcia istnieje tak samo jak Klient
        /// </summary>
        /// <param name="Objekt"></param>
        /// <param name="Biletdousuniecia"></param>
        public void AnulujRezerwacje(Klient Objekt, RezerwcjaBilet Biletdousuniecia)
        {     
            LNIDRezerwacjiBiletow.Add(Biletdousuniecia.GetNrRezerwacji());// numer biletu jest wsadzany do kosza, z którego później będzie brane 
            ListaRezerwacji.Remove(Biletdousuniecia);
            Objekt.UsunBiletRezerwacje(Biletdousuniecia);

        }

    }
}
