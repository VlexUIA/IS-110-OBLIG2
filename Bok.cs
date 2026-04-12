namespace IS110OBLIG1
{
    class Bok : IPrintbar
    {
        // init - kan bare settes når objektet lages
        public string ID             { get; init; }
        public string Tittel         { get; init; }
        public string Forfatter      { get; init; }
        public int Utgivelsesar      { get; init; }
        public int AntallEksemplarer { get; init; }

        // private set - Utlant kan bare endres via LanUt() og Returner() nedenfor
        public int Utlant { get; private set; } = 0;

        public Bok(string id, string tittel, string forfatter, int ar, int antall)
        {
            ID                = id;
            Tittel            = tittel;
            Forfatter         = forfatter;
            Utgivelsesar      = ar;
            AntallEksemplarer = antall;
        }

        // Regner ut hvor mange eksemplarer som er ledige
        public int Tilgjengelige()
        {
            return AntallEksemplarer - Utlant;
        }

        // Låner ut ett eksemplar - returnerer false hvis ingen er ledige
        public bool LanUt()
        {
            if (Tilgjengelige() <= 0)
            {
                return false; // ingen ledige eksemplarer
            }

            Utlant++; // øker antall utlånte med 1
            return true;
        }

        // Returnerer ett eksemplar tilbake
        public void Returner()
        {
            if (Utlant > 0)
            {
                Utlant--; // reduserer antall utlånte med 1
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine("[" + ID + "] " + Tittel + " av " + Forfatter + " (" + Utgivelsesar + ") | Tilgjengelig: " + Tilgjengelige() + "/" + AntallEksemplarer);
        }
    }
}
