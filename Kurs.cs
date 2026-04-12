namespace IS110OBLIG1
{
    class Kurs : IPrintbar
    {
        // init - kan bare settes når objektet lages
        public string Kode        { get; init; }
        public string Navn        { get; init; }
        public double Studiepoeng { get; init; }
        public int MaksPlasser    { get; init; }
        public string FaglarerID  { get; init; }

        // private set - disse listene kan bare endres via metodene nedenfor
        public List<Student> Deltakere { get; private set; } = new List<Student>();
        public List<string> Pensum     { get; private set; } = new List<string>();

        public Kurs(string kode, string navn, double studiepoeng, int maksPlasser, string faglarerID)
        {
            Kode        = kode;
            Navn        = navn;
            Studiepoeng = studiepoeng;
            MaksPlasser = maksPlasser;
            FaglarerID  = faglarerID;
        }

        // Melder en student på kurset
        // Returnerer en feilmelding som tekst hvis noe går galt, eller null hvis alt er ok
        public string? MeldPa(Student student)
        {
            // Sjekk om kurset er fullt
            if (Deltakere.Count >= MaksPlasser)
            {
                return "Kurset er fullt!";
            }

            // LINQ: sjekk om studenten allerede er påmeldt
            bool alleredePameldt = Deltakere.Any(d => d.StudentID == student.StudentID);
            if (alleredePameldt)
            {
                return student.Navn + " er allerede påmeldt " + Navn;
            }

            // Alt er ok - legg til studenten
            Deltakere.Add(student);
            student.MeldPaKurs(Kode);
            return null; // null betyr ingen feil
        }

        // Melder en student av kurset
        // Returnerer feilmelding eller null hvis ok
        public string? MeldAv(Student student)
        {
            // LINQ: finn studenten i deltakerslisten
            Student? funnet = Deltakere.FirstOrDefault(d => d.StudentID == student.StudentID);

            if (funnet == null)
            {
                return "Student er ikke påmeldt dette kurset.";
            }

            // Fjern studenten fra kurset og kurset fra studenten
            Deltakere.Remove(funnet);
            student.MeldAvKurs(Kode);
            return null;
        }

        // Setter karakter på en student i dette kurset
        public string? SettKarakter(string studentID, string karakter, List<Student> alleStudenter)
        {
            // LINQ: finn studenten i deltakerlisten
            Student? student = Deltakere.FirstOrDefault(d => d.StudentID == studentID);

            if (student == null)
            {
                return "Fant ikke studenten i dette kurset.";
            }

            // Sjekk at karakteren er gyldig
            string[] gyldigeKarakterer = { "A", "B", "C", "D", "E", "F" };
            if (!gyldigeKarakterer.Contains(karakter.ToUpper()))
            {
                return "Ugyldig karakter. Du må bruke A, B, C, D, E eller F.";
            }

            student.SettKarakter(Kode, karakter.ToUpper());
            return null;
        }

        // Legger til en bok i pensumlisten for dette kurset
        public void LeggTilPensum(string bok)
        {
            if (!Pensum.Contains(bok))
            {
                Pensum.Add(bok);
            }
        }

        // Skriver ut all info om kurset
        public void PrintInfo()
        {
            Console.WriteLine("Kurs: " + Kode + " - " + Navn + " (" + Studiepoeng + " sp) | Plasser: " + Deltakere.Count + "/" + MaksPlasser + " | Faglærer: " + FaglarerID);

            // Skriv ut alle deltakere
            foreach (Student s in Deltakere)
            {
                Console.WriteLine("  - " + s.Navn);
            }

            // Skriv ut pensum hvis det finnes
            if (Pensum.Count > 0)
            {
                Console.WriteLine("  Pensum:");
                foreach (string p in Pensum)
                {
                    Console.WriteLine("    * " + p);
                }
            }
        }
    }
}
