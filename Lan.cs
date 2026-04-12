namespace IS110OBLIG1
{
    class Lan : IPrintbar
    {
        // init - kan bare settes når objektet lages
        public string LanID      { get; init; }
        public string BrukerID   { get; init; }
        public string BrukerNavn { get; init; }
        public string BokTittel  { get; init; }
        public string BokID      { get; init; }

        // private set - ErAktivt kan bare endres via Avslutt() nedenfor
        // Starter som true fordi lånet er aktivt når det opprettes
        public bool ErAktivt { get; private set; } = true;

        public Lan(string lanID, string brukerID, string brukerNavn, string bokTittel, string bokID)
        {
            LanID      = lanID;
            BrukerID   = brukerID;
            BrukerNavn = brukerNavn;
            BokTittel  = bokTittel;
            BokID      = bokID;
        }

        // Avslutter lånet når boken returneres
        public void Avslutt()
        {
            ErAktivt = false;
        }

        public void PrintInfo()
        {
            // Ternær operator - kort måte å skrive if/else på
            // Hvis ErAktivt er true → "Aktiv", ellers → "Returnert"
            string status = ErAktivt ? "Aktiv" : "Returnert";
            Console.WriteLine("[" + LanID + "] " + BokTittel + " -> " + BrukerNavn + " | Status: " + status);
        }
    }
}
