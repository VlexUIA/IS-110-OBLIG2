namespace IS110OBLIG1
{
    // Abstrakt klasse - en felles "mal" for alle brukere i systemet
    // Vi kan ikke lage et Bruker-objekt direkte, men Student, Faglarer
    // og Bibliotekansatt arver fra denne og får alt som er her
    abstract class Bruker
    {
        // init betyr at disse kun kan settes når objektet lages - ikke endres etterpå
        public string BrukerId { get; init; }
        public string Navn     { get; init; }
        public string Epost    { get; init; }
        public Rolle Rolle     { get; init; }

        // Passord har private set - kan bare leses utenfra, ikke endres
        public string Passord  { get; private set; }

        // Konstruktøren kjører når vi lager en ny bruker
        // protected betyr at bare denne klassen og klasser som arver kan bruke den
        protected Bruker(string id, string navn, string epost, string passord, Rolle rolle)
        {
            BrukerId = id;
            Navn     = navn;
            Epost    = epost;
            Passord  = passord;
            Rolle    = rolle;
        }

        // Sjekker om passordet brukeren skreiv inn stemmer
        public bool SjekkPassord(string passord)
        {
            return Passord == passord;
        }

        // abstract betyr at alle klasser som arver fra Bruker må lage sin egen PrintInfo()
        public abstract void PrintInfo();
    }
}
