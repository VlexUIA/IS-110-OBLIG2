namespace IS110OBLIG1
{
    // Bibliotekansatt arver fra Bruker
    class Bibliotekansatt : Bruker
    {
        public string AnsattID { get; init; }

        public Bibliotekansatt(string id, string navn, string epost, string passord)
            : base(id, navn, epost, passord, Rolle.Bibliotekansatt)
        {
            AnsattID = id;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Bibliotekansatt: " + Navn + " | ID: " + AnsattID);
        }
    }
}
