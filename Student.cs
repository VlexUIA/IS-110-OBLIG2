namespace IS110OBLIG1
{
    // Student arver fra Bruker - får BrukerID, Navn, Epost, Passord og Rolle
    class Student : Bruker
    {
        // init - kan bare settes når objektet lages
        public string StudentID { get; init; }

        // private - istene kan bare endres via metodene nedenfor
        public List<string> PamelteKurs { get; private set; } = new List<string>();
        public List<string> AktiveLan   { get; private set; } = new List<string>();

        // Dictionary er som en liste, men har nøkkel og verdi
        // Her lagres kurskode -> karakter = "INF101" -> "B+"
        private Dictionary<string, string> karakterer = new Dictionary<string, string>();

        // Konstruktøren kaller base() som er Bruker sin konstruktør
        public Student(string id, string navn, string epost, string passord)
            : base(id, navn, epost, passord, Rolle.Student)
        {
            StudentID = id;
        }

        // Melder studenten på et kurs - returnerer false hvis allerede påmeldt
        public bool MeldPaKurs(string kurskode)
        {
            if (PamelteKurs.Contains(kurskode))
            {
                return false; // allerede påmeldt
            }

            PamelteKurs.Add(kurskode);
            return true;
        }

        // Melder studenten av et kurs - returnerer false hvis ikke påmeldt
        public bool MeldAvKurs(string kurskode)
        {
            if (!PamelteKurs.Contains(kurskode))
            {
                return false; // ikke påmeldt
            }

            PamelteKurs.Remove(kurskode);
            return true;
        }

        // Legger til et lån-ID i listen over aktive lån
        public void LeggTilLan(string lanID)
        {
            AktiveLan.Add(lanID);
        }

        // Fjerner et lån-ID når boken er returnert (Lx -1)
        public void FjernLan(string lanID)
        {
            AktiveLan.Remove(lanID);
        }

        // Lagrer en karakter for et bestemt kurs
        public void SettKarakter(string kurskode, string karakter)
        {
            karakterer[kurskode] = karakter;
        }

        // override betyr at vi lager vår egen versjon av PrintInfo() fra Bruker
        public override void PrintInfo()
        {
            Console.WriteLine("Student: " + Navn + " | ID: " + StudentID + " | Epost: " + Epost);
        }

        // Skriver ut alle karakterer studenten har fått
        public void PrintKarakterer()
        {
            if (karakterer.Count == 0)
            {
                Console.WriteLine("Ingen karakterer registrert ennå.");
                return;
            }

            Console.WriteLine("Dine karakterer:");
            foreach (var k in karakterer)
            {
                // k.Key er kurskoden, k.Value er karakteren
                Console.WriteLine("  " + k.Key + " -> " + k.Value);
            }
        }
    }
}
