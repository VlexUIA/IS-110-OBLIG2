namespace IS110OBLIG1
{
    // Utvekslingsstudent arver fra Student
    // Den får med seg ALT Student har, pluss sine egne egenskaper
    class Utvekslingsstudent : Student
    {
        public string Hjemuniversitet { get; init; }
        public string Land            { get; init; }
        public string Periode         { get; init; }

        // base() kaller Student sin konstruktør med id, navn, epost og passord
        public Utvekslingsstudent(string id, string navn, string epost, string passord,
            string hjemuniversitet, string land, string periode)
            : base(id, navn, epost, passord)
        {
            Hjemuniversitet = hjemuniversitet;
            Land            = land;
            Periode         = periode;
        }

        // override - lager vår egen versjon av PrintInfo()
        // Polymorfisme: samme metode, men oppfører seg annerledes enn Student sin versjon
        public override void PrintInfo()
        {
            // Kaller først Student sin PrintInfo() som skriver ut vanlig studentinfo
            base.PrintInfo();
            // Så legger vi til ekstra info for utvekslingsstudenter
            Console.WriteLine("  Hjemuniversitet: " + Hjemuniversitet + " | Land: " + Land + " | Periode: " + Periode);
        }
    }
}
