namespace IS110OBLIG1
{
    // Faglarer arver fra Bruker - får med seg BrukerID, Navn, Epost, Passord og Rolle
    class Faglarer : Bruker
    {
        public string AnsattID { get; init; }
        public string Avdeling { get; init; }

        // private set - listene kan bare endres via metodene nedenfor
        public List<string> MineKurs  { get; private set; } = new List<string>();
        public List<string> AktiveLan { get; private set; } = new List<string>();

        public Faglarer(string id, string navn, string epost, string passord, string avdeling)
            : base(id, navn, epost, passord, Rolle.Faglarer)
        {
            AnsattID  = id;
            Avdeling  = avdeling;
        }

        // Legger til et kurs faglæreren underviser
        public void LeggTilKurs(string kurskode)
        {
            // Sjekker at kurset ikke allerede er i listen
            if (!MineKurs.Contains(kurskode))
            {
                MineKurs.Add(kurskode);
            }
        }

        // Legger til et lån-ID når faglæreren låner en bok
        public void LeggTilLan(string lanID)
        {
            AktiveLan.Add(lanID);
        }

        // Fjerner et lån-ID når boken er returnert
        public void FjernLan(string lanID)
        {
            AktiveLan.Remove(lanID);
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Faglærer: " + Navn + " | ID: " + AnsattID + " | Avdeling: " + Avdeling);
        }
    }
}
