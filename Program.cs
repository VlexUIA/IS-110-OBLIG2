namespace IS110OBLIG1
{
    // Enum for studentener - tallene = det brukeren taster inn
    // MeldPaKurs = 1 betyr at valg 1 er "Meld på kurs" osv.
    enum StudentMeny
    {
        MeldPaKurs   = 1,
        MeldAvKurs   = 2,
        SeKarakterer = 3,
        SokBok       = 4,
        LanBok       = 5,
        ReturnerBok  = 6,
        MineKurs     = 7,
        Avslutt      = 0
    }

    // Enum for faglærere
    enum FaglarerMeny
    {
        OpprettKurs     = 1,
        SokKurs         = 2,
        SokBok          = 3,
        LanBok          = 4,
        ReturnerBok     = 5,
        SettKarakter    = 6,
        RegistrerPensum = 7,
        Avslutt         = 0
    }

    // Enum for biblioteketet
    enum BibMeny
    {
        RegistrerBok   = 1,
        SeAktiveLan    = 2,
        SeLanHistorikk = 3,
        Avslutt        = 0
    }

    class Program
    {
        // Static liste som lagrer all data mens programmet kjører
        static List<Bruker> brukere   = new List<Bruker>();
        static List<Kurs>   kursliste = new List<Kurs>();
        static List<Bok>    bokliste  = new List<Bok>();
        static List<Lan>    lanliste  = new List<Lan>();

        // Teller som gir hvert lån et nummer (L1, L2, L3)
        static int lanTeller = 1;

        static void Main()
        {
            // Legger inn eksempler
            brukere.Add(new Student("S001", "Narceli Majcher", "narcelim@uni.no", "marcelim123"));
            brukere.Add(new Student("S002", "Dawid P. Pasinski", "dawidpp@uni.no", "dawidpp123"));
            brukere.Add(new Student("S003", "Michael Stiga", "Michsti@uni.no", "stigi123"));
            brukere.Add(new Utvekslingsstudent("US001", "Max Verstappen", "maxver@wars.pl", "passord123", "Warsaw", "POL", "Jan-Jun 2027"));
            brukere.Add(new Faglarer("F001", "Dawid Rød", "dawidr@uni.no", "passord123", "IT og Informatikk"));
            brukere.Add(new Faglarer("F002", "Michael Jay", "mikijayr@uni.no", "passord123", "IT og Programmering"));
            brukere.Add(new Bibliotekansatt("B001", "Sohib Ferhan", "Sohibfer@uni.no", "passord123"));
            kursliste.Add(new Kurs("INF101", "Innføring i programmering", 10, 30, "F001"));
            kursliste.Add(new Kurs("IS-105", "C# med gutta", 10, 30, "F002"));
            bokliste.Add(new Bok("B001", "C# med jenta", "Wadar Dose", 2022, 3));

            // Bruker? med spørsmålstegn betyr at innlogget kan være null
            Bruker? innlogget = LoggInn();

            // Hvis innlogging feilet eller brukeren avsluttet
            if (innlogget == null)
            {
                Console.WriteLine("Avslutter programmet.");
                return;
            }

            Console.WriteLine("\nVelkommen, " + innlogget.Navn + "!");

            // Kjør riktig meny basert på rolle
            // switch sjekker hvilken Rolle innlogget har og går til riktig case
            switch (innlogget.Rolle)
            {
                case Rolle.Student:
                    // Caster (konverterer) Bruker til Student så vi får tilgang til Student-metoder
                    KjorStudentMeny((Student)innlogget);
                    break;
                case Rolle.Faglarer:
                    KjorFaglarerMeny((Faglarer)innlogget);
                    break;
                case Rolle.Bibliotekansatt:
                    KjorBibliotekMeny((Bibliotekansatt)innlogget);
                    break;
            }

            Console.WriteLine("Ha en fin dag!");
        }
        
        // ==========================
        // INNLOGGING OG REGISTRERING
        // ==========================
        
        static Bruker? LoggInn()
        {
            Console.WriteLine("=== UNIVERSITETSSYSTEM ===");
            Console.WriteLine("[1] Logg inn");
            Console.WriteLine("[2] Registrer ny bruker");
            Console.WriteLine("[0] Avslutt");
            Console.Write("Valg: ");

            string valg = Console.ReadLine() ?? "";

            if (valg == "1")
            {
                // Hopper over til Autentiser og kjører (linje 122)
                return Autentiser();
            }
            else if (valg == "2")
            {
                // Hopper over til "Registrer og kjører (linje 150)
                return Registrer();
            }

            // Hvis brukeren valgte 0 eller noe annet, returner null
            return null;
        }
        
        static Bruker? Autentiser()
        {
            Console.Write("Epost: ");
            string epost = Console.ReadLine() ?? "";

            Console.Write("Passord: ");
            string passord = Console.ReadLine() ?? "";

            // LINQ: finn brukeren med denne eposten i listen av personer
            Bruker? bruker = brukere.FirstOrDefault(b => b.Epost == epost);

            if (bruker == null)
            {
                // Hvis ikke fant, informerer bruker
                Console.WriteLine("Fant ingen bruker med denne eposten.");
                return null;
            }

            // Sjekk om passordet stemmer
            if (!bruker.SjekkPassord(passord))
            {
                Console.WriteLine("Feil passord.");
                return null;
            }

            return bruker;
        }

        static Bruker? Registrer()
        {
            Console.WriteLine("\nHvilken type bruker vil du registrere?");
            Console.WriteLine("[1] Student");
            Console.WriteLine("[2] Faglærer");
            Console.WriteLine("[3] Bibliotekansatt");
            Console.Write("Valg: ");

            string rolleValg = Console.ReadLine() ?? "";

            Console.Write("Navn: ");
            string navn = Console.ReadLine() ?? "";

            Console.Write("Epost: ");
            string epost = Console.ReadLine() ?? "";

            // Sjekk at ingen andre har denne eposten - LINQ Any() sjekker om noen matcher
            if (brukere.Any(b => b.Epost == epost))
            {
                Console.WriteLine("En bruker med denne eposten finnes allerede.");
                return null;
            }

            Console.Write("Passord: ");
            string passord = Console.ReadLine() ?? "";

            // IsNullOrWhiteSpace sjekker om teksten er tom eller bare mellomrom
            if (string.IsNullOrWhiteSpace(passord))
            {
                Console.WriteLine("Passord kan ikke være tomt.");
                return null;
            }

            // Lag et unikt ID basert på antall brukere som allerede finnes
            string id = "ID" + (brukere.Count + 1);

            // Bruker? med spørsmålstegn - kan være null hvis rolleValg er ugyldig
            Bruker? nyBruker = null;

            if (rolleValg == "1")
            {
                nyBruker = new Student(id, navn, epost, passord);
            }
            else if (rolleValg == "2")
            {
                Console.Write("Avdeling: ");
                string avdeling = Console.ReadLine() ?? "";
                nyBruker = new Faglarer(id, navn, epost, passord, avdeling);
            }
            else if (rolleValg == "3")
            {
                nyBruker = new Bibliotekansatt(id, navn, epost, passord);
            }
            else
            {
                Console.WriteLine("Ugyldig valg.");
                return null;
            }

            brukere.Add(nyBruker);
            Console.WriteLine("Bruker registrert! Du kan nå logge inn.");
            return nyBruker;
        }

        // ===========
        // STUDENTMENY
        // ===========

        static void KjorStudentMeny(Student student)
        {
            bool kjorer = true;
            
            // Meny med valgene en student kan ta    
            while (kjorer)
            {
                Console.WriteLine("\n=== STUDENTMENY ===");
                Console.WriteLine("[1] Meld på kurs");
                Console.WriteLine("[2] Meld av kurs");
                Console.WriteLine("[3] Se karakterer");
                Console.WriteLine("[4] Søk bok");
                Console.WriteLine("[5] Lån bok");
                Console.WriteLine("[6] Returner bok");
                Console.WriteLine("[7] Se mine kurs");
                Console.WriteLine("[0] Avslutt");
                Console.Write("Valg: ");

                // int.TryParse krasjer ikke hvis brukeren taster inn tekst
                // out int valg betyr at resultatet lagres i variabelen "valg"
                if (!int.TryParse(Console.ReadLine(), out int valg))
                {
                    Console.WriteLine("Ugyldig valg. Skriv inn et tall.");
                    continue; // = hopp til toppen av while-løkken
                }

                /* Student Caser, for hvert valg brukeren tar,
                konsolen har en case for hvert valg som begynner å kjøre det brukeren har valgt. */
                switch ((StudentMeny)valg)
                {
                    case StudentMeny.MeldPaKurs:
                        MeldStudentPaKurs(student);
                        break;
                    case StudentMeny.MeldAvKurs:
                        MeldStudentAvKurs(student);
                        break;
                    case StudentMeny.SeKarakterer:
                        student.PrintKarakterer();
                        break;
                    case StudentMeny.SokBok:
                        SokBok();
                        break;
                    case StudentMeny.LanBok:
                        LanBok(student.StudentID, student.Navn, student);
                        break;
                    case StudentMeny.ReturnerBok:
                        ReturnerBok(student.StudentID, student);
                        break;
                    case StudentMeny.MineKurs:
                        SeMineKurs(student);
                        break;
                    case StudentMeny.Avslutt:
                        kjorer = false;
                        break;
                    default:
                        Console.WriteLine("Ugyldig valg.");
                        break;
                }
            }
        }

        // ============
        // FAGLÆRERMENY
        // ============

        static void KjorFaglarerMeny(Faglarer faglarer)
        {
            bool kjorer = true;

            while (kjorer)
            {
                Console.WriteLine("\n=== FAGLÆRERMENY ===");
                Console.WriteLine("[1] Opprett kurs");
                Console.WriteLine("[2] Søk kurs");
                Console.WriteLine("[3] Søk bok");
                Console.WriteLine("[4] Lån bok");
                Console.WriteLine("[5] Returner bok");
                Console.WriteLine("[6] Sett karakter på student");
                Console.WriteLine("[7] Registrer pensum til kurs");
                Console.WriteLine("[0] Avslutt");
                Console.Write("Valg: ");

                if (!int.TryParse(Console.ReadLine(), out int valg))
                {
                    Console.WriteLine("Ugyldig valg. Skriv inn et tall.");
                    continue;
                }
                /* Faglærer Caser, for hvert valg brukeren tar,
                konsolen har en case for hvert valg som begynner å kjøre det brukeren har valgt. */

                switch ((FaglarerMeny)valg)
                {
                    case FaglarerMeny.OpprettKurs:
                        OpprettKurs(faglarer);
                        break;
                    case FaglarerMeny.SokKurs:
                        SokKurs();
                        break;
                    case FaglarerMeny.SokBok:
                        SokBok();
                        break;
                    case FaglarerMeny.LanBok:
                        LanBok(faglarer.AnsattID, faglarer.Navn, faglarer);
                        break;
                    case FaglarerMeny.ReturnerBok:
                        ReturnerBok(faglarer.AnsattID, faglarer);
                        break;
                    case FaglarerMeny.SettKarakter:
                        SettKarakter(faglarer);
                        break;
                    case FaglarerMeny.RegistrerPensum:
                        RegistrerPensum(faglarer);
                        break;
                    case FaglarerMeny.Avslutt:
                        kjorer = false;
                        break;
                    default:
                        Console.WriteLine("Ugyldig valg.");
                        break;
                }
            }
        }

        // =============
        // BIBLIOTE KMENY
        // =============

        static void KjorBibliotekMeny(Bibliotekansatt ansatt)
        {
            bool kjorer = true;

            while (kjorer)
            {
                Console.WriteLine("\n=== BIBLIOTEKMENY ===");
                Console.WriteLine("[1] Registrer ny bok");
                Console.WriteLine("[2] Se aktive lån");
                Console.WriteLine("[3] Se lånehistorikk");
                Console.WriteLine("[0] Avslutt");
                Console.Write("Valg: ");

                if (!int.TryParse(Console.ReadLine(), out int valg))
                {
                    Console.WriteLine("Ugyldig valg. Skriv inn et tall.");
                    continue;
                }
                
                /* Bibliotek Caser, for hvert valg brukeren tar,
                konsolen har en case for hvert valg som begynner å kjøre det brukeren har valgt. */
                switch ((BibMeny)valg)
                {
                    case BibMeny.RegistrerBok:
                        RegistrerBok();
                        break;
                    case BibMeny.SeAktiveLan:
                        SeAktiveLan();
                        break;
                    case BibMeny.SeLanHistorikk:
                        SeLanHistorikk();
                        break;
                    case BibMeny.Avslutt:
                        kjorer = false;
                        break;
                    default:
                        Console.WriteLine("Ugyldig valg.");
                        break;
                }
            }
        }

        // ==============
        // KURSFUNKSJONER
        // ==============

        static void OpprettKurs(Faglarer faglarer)
        {
            Console.Write("Kurskode: ");
            string kode = Console.ReadLine() ?? "";

            // Sjekk at kurskoden ikke er tom
            if (string.IsNullOrWhiteSpace(kode))
            {
                Console.WriteLine("Kurskode kan ikke være tom.");
                return;
            }

            // LINQ: sjekk at ingen andre kurs har samme kode
            if (kursliste.Any(k => k.Kode.ToLower() == kode.ToLower()))
            {
                Console.WriteLine("Et kurs med denne koden finnes allerede.");
                return;
            }

            // Console spør hvilket kursnavn og lagrer informasjon brukeren har skrevet inn.
            Console.Write("Kursnavn: ");
            string navn = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(navn))
            {
                Console.WriteLine("Kursnavn kan ikke være tomt.");
                return;
            }

            // LINQ: sjekk at ingen andre kurs har samme navn
            if (kursliste.Any(k => k.Navn.ToLower() == navn.ToLower()))
            {
                Console.WriteLine("Et kurs med dette navnet finnes allerede.");
                return;
            }

            Console.Write("Studiepoeng: ");
            // TryParse prøver å gjøre om tekst til tall - returnerer false hvis det ikke går
            if (!double.TryParse(Console.ReadLine(), out double sp) || sp <= 0)
            {
                Console.WriteLine("Ugyldig antall studiepoeng. Må være større enn 0.");
                return;
            }

            Console.Write("Maks plasser: ");
            if (!int.TryParse(Console.ReadLine(), out int maks) || maks <= 0)
            {
                Console.WriteLine("Ugyldig antall plasser. Må være større enn 0.");
                return;
            }

            // Lag kurset og legg det til i lister
            Kurs nyttKurs = new Kurs(kode, navn, sp, maks, faglarer.AnsattID);
            kursliste.Add(nyttKurs);
            faglarer.LeggTilKurs(kode);
            Console.WriteLine("Kurs opprettet!");
        }

        static void MeldStudentPaKurs(Student student)
        {
            if (kursliste.Count == 0)
            {
                Console.WriteLine("Det finnes ingen kurs å melde seg på.");
                return;
            }

            // Viser alle tilgjengelige kurs
            Console.WriteLine("Tilgjengelige kurs:");
            foreach (Kurs k in kursliste)
            {
                k.PrintInfo();
            }

            Console.Write("Skriv inn kurskode: ");
            string kode = Console.ReadLine() ?? "";

            // LINQ: finn kurset med denne koden
            Kurs? kurs = kursliste.FirstOrDefault(k => k.Kode == kode);
            if (kurs == null)
            {
                Console.WriteLine("Fant ikke kurset.");
                return;
            }

            // MeldPa() i Kurs-klassen håndterer logikken og returnerer feilmelding eller null
            string? feil = kurs.MeldPa(student);
            if (feil != null)
            {
                Console.WriteLine(feil);
            }
            else
            {
                Console.WriteLine(student.Navn + " er nå meldt på " + kurs.Navn);
            }
        }

        static void MeldStudentAvKurs(Student student)
        {
            // = 0 betyr at studenten ikke er påmeldt på noe
            if (student.PamelteKurs.Count == 0)
            {
                Console.WriteLine("Du er ikke påmeldt noen kurs.");
                return;
            }

            // Vis hvilke kurs studenten er påmeldt
            Console.WriteLine("Du er påmeldt disse kursene: " + string.Join(", ", student.PamelteKurs));
            Console.Write("Skriv inn kurskode du vil melde deg av: ");
            string kode = Console.ReadLine() ?? "";

            
            // Sjekker om koden finnes og gir den en kode - hvis ikke, gir beskjed og stopper program
            Kurs? kurs = kursliste.FirstOrDefault(k => k.Kode == kode);
            if (kurs == null)
            {
                Console.WriteLine("Fant ikke kurset.");
                return;
            }

            // Prøver å melde studenten av kursert, hvis noe går galt - sender ut en feilmelding.
            string? feil = kurs.MeldAv(student);
            if (feil != null)
            {
                Console.WriteLine(feil);
            }
            else
            {
                Console.WriteLine(student.Navn + " er meldt av " + kurs.Navn);
            }
        }

        static void SeMineKurs(Student student)
        {
            if (student.PamelteKurs.Count == 0)
            {
                Console.WriteLine("Du er ikke påmeldt noen kurs.");
                return;
            }

            Console.WriteLine("Dine påmeldte kurs:");
            foreach (string kode in student.PamelteKurs)
            {
                // LINQ: finn kursobjektet med denne koden
                Kurs? k = kursliste.FirstOrDefault(k => k.Kode == kode);
                if (k != null)
                {
                    k.PrintInfo();
                }
            }
        }

        static void SokKurs()
        {
            Console.Write("Søk etter kurs (kode eller navn): ");
            // ToLower() gjør teksten til lowercase så søket ikke er case-sensitivt
            string sok = Console.ReadLine()?.ToLower() ?? "";

            // LINQ Where: finn alle kurs der kode eller navn inneholder søketeksten
            List<Kurs> treff = kursliste
                .Where(k => k.Kode.ToLower().Contains(sok) || k.Navn.ToLower().Contains(sok))
                .ToList();

            // Hvis treff = 0, Ingen kurs funnet.
            if (treff.Count == 0)
            {
                Console.WriteLine("Ingen kurs funnet.");
                return;
            }

            // For hver kurs funnet, skriv ut hver kurs.
            foreach (Kurs k in treff)
            {
                k.PrintInfo();
            }
        }

        static void SettKarakter(Faglarer faglarer)
        {
            // LINQ: finn kun kurs som denne faglæreren underviser
            List<Kurs> mineKurs = kursliste.Where(k => k.FaglarerID == faglarer.AnsattID).ToList();

            if (mineKurs.Count == 0)
            {
                Console.WriteLine("Du underviser ingen kurs ennå.");
                return;
            }
            
            // Skriver ut hver kurs du underviser.
            Console.WriteLine("Dine kurs:");
            foreach (Kurs k in mineKurs)
            {
                k.PrintInfo();
            }

            Console.Write("Kurskode: ");
            string kode = Console.ReadLine() ?? "";

            // LINQ: finn kurset blant faglærerens kurs
            Kurs? kurs = mineKurs.FirstOrDefault(k => k.Kode == kode);
            if (kurs == null)
            {
                Console.WriteLine("Fant ikke kurset, eller du har ikke tilgang til det.");
                return;
            }

            Console.Write("Student-ID: ");
            string studentID = Console.ReadLine() ?? "";

            Console.Write("Karakter (A, B, C, D, E eller F): ");
            string karakter = Console.ReadLine() ?? "";

            // Hent alle studenter fra brukerlisten med OfType<Student>()
            List<Student> alleStudenter = brukere.OfType<Student>().ToList();
            
            // Prøver å sette karakter for studenten i kurset.
            // Hvis det feiler, skrives feilmeldingen ut.
            // Hvis det lykkes, bekreftes med writeline.
            string? feil = kurs.SettKarakter(studentID, karakter, alleStudenter);
            if (feil != null)
            {
                Console.WriteLine(feil);
            }
            else
            {
                Console.WriteLine("Karakter satt!");
            }
        }

        static void RegistrerPensum(Faglarer faglarer)
        {
            // LINQ: finn kun kurs som denne faglæreren underviser
            List<Kurs> mineKurs = kursliste.Where(k => k.FaglarerID == faglarer.AnsattID).ToList();

            if (mineKurs.Count == 0)
            {
                Console.WriteLine("Du underviser ingen kurs ennå.");
                return;
            }

            Console.WriteLine("Dine kurs:");
            foreach (Kurs k in mineKurs)
            {
                Console.WriteLine("  " + k.Kode + " - " + k.Navn);
            }

            Console.Write("Kurskode: ");
            string kode = Console.ReadLine() ?? "";

            Kurs? kurs = mineKurs.FirstOrDefault(k => k.Kode == kode);
            if (kurs == null)
            {
                Console.WriteLine("Fant ikke kurset, eller du har ikke tilgang til det.");
                return;
            }

            Console.Write("Skriv inn pensumbok (tittel): ");
            string pensum = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(pensum))
            {
                Console.WriteLine("Pensum kan ikke være tomt.");
                return;
            }

            kurs.LeggTilPensum(pensum);
            Console.WriteLine("Pensum lagt til!");
        }

        // =============
        // BOKFUNKSJONER
        // =============

        static void SokBok()
        {
            // ToLower = Programmet bryr seg ikke om du skriver med capslock eller ikke.
            Console.Write("Søk etter bok (tittel eller forfatter): ");
            string sok = Console.ReadLine()?.ToLower() ?? "";

            // LINQ Where: finn alle bøker der tittel eller forfatter inneholder søketeksten
            List<Bok> treff = bokliste
                .Where(b => b.Tittel.ToLower().Contains(sok) || b.Forfatter.ToLower().Contains(sok))
                .ToList();

            if (treff.Count == 0)
            {
                Console.WriteLine("Ingen bøker funnet.");
                return;
            }

            foreach (Bok b in treff)
            {
                b.PrintInfo();
            }
        }

        // brukerObj er enten en Student eller Faglarer
        // object er den mest generelle typen i C# - alle klasser er også av typen object
        // LanBok til BrukerID, Navnet og BrukerObj.
        static void LanBok(string brukerID, string brukerNavn, object brukerObj)
        {
            // LINQ: vis kun bøker som har ledige eksemplarer
            List<Bok> tilgjengelige = bokliste.Where(b => b.Tilgjengelige() > 0).ToList();

            if (tilgjengelige.Count == 0)
            {
                Console.WriteLine("Ingen bøker er tilgjengelig for utlån akkurat nå.");
                return;
            }

            Console.WriteLine("Tilgjengelige bøker:");
            // For hver tilgjenlig bok, skriver ut hver bok.
            foreach (Bok b in tilgjengelige)
            {
                b.PrintInfo();
            }

            Console.Write("Bok-ID: ");
            string bokID = Console.ReadLine() ?? "";

            // LINQ: finn boken med denne ID-en
            Bok? bok = bokliste.FirstOrDefault(b => b.ID == bokID);
            if (bok == null)
            {
                Console.WriteLine("Fant ikke boken.");
                return;
            }

            // LanUt() i Bok-klassen håndterer logikken - returnerer false hvis ingen er ledige
            if (!bok.LanUt())
            {
                Console.WriteLine("Ingen eksemplarer tilgjengelig.");
                return;
            }

            // Lag et nytt lån med unikt ID
            string lanID = "L" + lanTeller;
            lanTeller++; // neste lån får neste nummer

            Lan nyttLan = new Lan(lanID, brukerID, brukerNavn, bok.Tittel, bok.ID);
            lanliste.Add(nyttLan);

            // Oppdater brukerens liste over aktive lån
            // "is" sjekker hvilken type brukerObj egentlig er
            if (brukerObj is Student s)
            {
                s.LeggTilLan(lanID);
            }
            else if (brukerObj is Faglarer f)
            {
                f.LeggTilLan(lanID);
            }

            Console.WriteLine("Boken er lånt ut! Ditt lån-ID er: " + lanID);
        }

        static void ReturnerBok(string brukerID, object brukerObj)
        {
            // LINQ: finn kun aktive lån som tilhører denne brukeren
            List<Lan> mineLan = lanliste
                .Where(l => l.ErAktivt && l.BrukerID == brukerID)
                .ToList();

            if (mineLan.Count == 0)
            {
                Console.WriteLine("Du har ingen aktive lån.");
                return;
            }

            // Skriver ut hvert bok DU har utlånt.
            Console.WriteLine("Dine aktive lån:");
            foreach (Lan l in mineLan)
            {
                l.PrintInfo();
            }

            Console.Write("Lån-ID: ");
            string lanID = Console.ReadLine() ?? "";

            // LINQ: finn lånet med dette ID-et
            Lan? lan = mineLan.FirstOrDefault(l => l.LanID == lanID);
            if (lan == null)
            {
                Console.WriteLine("Fant ikke lånet.");
                return;
            }

            // Avslutt lånet og returner boken
            lan.Avslutt();

            // LINQ: finn boken og returner den
            // ?. betyr "gjør dette bare hvis bok ikke er null"
            Bok? bok = bokliste.FirstOrDefault(b => b.ID == lan.BokID);
            bok?.Returner();

            // Fjern lånet fra brukerens aktive lån
            if (brukerObj is Student s)
            {
                s.FjernLan(lanID);
            }
            else if (brukerObj is Faglarer f)
            {
                f.FjernLan(lanID);
            }

            Console.WriteLine("Bok returnert. Takk!");
        }

        static void RegistrerBok()
        {
            Console.Write("ID (f.eks. B002): ");
            string id = Console.ReadLine() ?? "";

            // Sjekk at ID ikke allerede er i bruk
            if (bokliste.Any(b => b.ID == id))
            {
                Console.WriteLine("En bok med denne ID-en finnes allerede.");
                return;
            }

            Console.Write("Tittel: ");
            string tittel = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(tittel))
            {
                Console.WriteLine("Tittel kan ikke være tom.");
                return;
            }

            Console.Write("Forfatter: ");
            string forfatter = Console.ReadLine() ?? "";

            // Utgivelsesår som er fra 1000 til 2027 sånn at tilfeldige tall ikke blir skrevet inn.
            Console.Write("Utgivelsesår: ");
            if (!int.TryParse(Console.ReadLine(), out int ar) || ar < 1500 || ar > 2027)
            {
                Console.WriteLine("Ugyldig utgivelsesår. Må være mellom 1500 og 2027.");
                return;
            }

            // Antall eksemplarer du vill låne ut, må være mer enn 0
            Console.Write("Antall eksemplarer: ");
            if (!int.TryParse(Console.ReadLine(), out int antall) || antall <= 0)
            {
                Console.WriteLine("Antall eksemplarer må være 1 eller mer.");
                return;
            }

            bokliste.Add(new Bok(id, tittel, forfatter, ar, antall));
            Console.WriteLine("Bok registrert!");
        }

        static void SeAktiveLan()
        {
            // LINQ: hent kun aktive lån
            List<Lan> aktive = lanliste.Where(l => l.ErAktivt).ToList();

            if (aktive.Count == 0)
            {
                Console.WriteLine("Ingen aktive lån for øyeblikket.");
                return;
            }

            Console.WriteLine("Aktive lån:");
            // Sjekker aktive lån og printer ut i konsolen til bruker.
            foreach (Lan l in aktive)
            {
                l.PrintInfo();
            }
        }

        static void SeLanHistorikk()
        {
            if (lanliste.Count == 0)
            {
                Console.WriteLine("Ingen lånehistorikk ennå.");
                return;
            }

            Console.WriteLine("All lånehistorikk:");
            foreach (Lan l in lanliste)
            {
                l.PrintInfo();
            }
        }
    }
}
