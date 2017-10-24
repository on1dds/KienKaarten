using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace kienkaart
{
    public static class init
    {
        public static int[] verplichteGetallen = { 1, 13, 22, 69 };
        public static int aantalKaarten = 150;
        public static string bestand = "kaarten.txt";
    }

    public class KienKaart
    {

        public List<int> GetallenOpKaart = new List<int>();
        Random k = new Random(Guid.NewGuid().GetHashCode());
        List<Rij> rijen = new List<Rij>();

        public void Nieuw()
        {
            GetallenOpKaart.Clear();

            // maak de rijen aan
            for (int getal = 0; getal < 6; getal++)
                rijen.Add(new Rij() { Kaart = this });

            // voeg de verplichte nummers toe
            foreach (int verplichtGetal in init.verplichteGetallen)
            {
                int rij = k.Next(0, 6);
                rijen[rij].kaartGetallen.Add(verplichtGetal);
                GetallenOpKaart.Add(verplichtGetal);
            }

            // vul aan met gewone nummers
            foreach (Rij rij in rijen)
                rij.GenerateRow();
        }

        public void PrintKaart(StreamWriter output)
        {
            foreach (Rij rij in rijen)
                output.WriteLine(rij.GetRowText());
        }
    }

    public class Rij
    {
        public List<int> kaartGetallen;
        public Random k = new Random(Guid.NewGuid().GetHashCode());
        public KienKaart Kaart;

        // Constructor
        public Rij()
        {
            kaartGetallen = new List<int>();
        }

        public string GetRowText()
        {
            string rijTekst = "";
            for (int i = 0; i < 9; i++)
                rijTekst += GetVeld(i) != -1 ? ";" + GetVeld(i).ToString(): ";";
            return rijTekst.Substring(1);
        }

        public void GenerateRow()
        {
            while (kaartGetallen.Count < 5)
            {
                int nieuwGetal = k.Next(1, 91);
                if (!IsTaken(nieuwGetal))
                {
                    kaartGetallen.Add(nieuwGetal);
                    Kaart.GetallenOpKaart.Add(nieuwGetal);
                }
            }
            kaartGetallen.Sort();
        }

        public bool Contains(int i)
        {
            foreach (int getal in kaartGetallen)
                if (i == getal)
                    return true;
            return false;
        }

        public bool IsTaken(int getal)
        {
            if (Kaart.GetallenOpKaart.Contains(getal))
                return true;

            int vraagveld = getal / 10;
            if (getal == 90) vraagveld = 8;

            foreach (int item in kaartGetallen)
            {
                int vglveld = item / 10;
                if (item == 90) vglveld = 8;

                if (vraagveld == vglveld)
                    return true;
            }
            return false;
        }
        public int GetVeld(int vraagveld)
        {
            foreach (int item in kaartGetallen)
            {
                int vglveld = item / 10;
                if (item == 90) vglveld = 8;
                if (vraagveld == vglveld) return item;
            }
            return -1;
        }
    }


    class Program
    {
        static List<KienKaart> kaarten = new List<KienKaart>();

        static void Main(string[] args)
        {
            StreamWriter bestand = new StreamWriter(init.bestand);
            for (int teller=0;teller<init.aantalKaarten;teller++)
            {
                KienKaart kaart = new KienKaart();
                kaart.Nieuw();
                kaarten.Add(kaart);
                kaart.PrintKaart(bestand);
                bestand.WriteLine("");
            }
            bestand.Close();
        }
    }
}
