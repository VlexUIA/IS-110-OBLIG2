using IS110OBLIG1;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IS110OBLIG1.Tests;

[TestClass]
[TestSubject(typeof(Bok))]
public class BokTest
{
    // TEST 1 - Tester at SjekkPassord() fungerer riktig
    [TestMethod]
    public void SjekkPassord_RiktigOgFeilPassord()
    {
        // Lag en student med passordet "hemmelig123"
        Student student = new Student("S001", "Narceli", "test@uni.no", "hemmelig123");

        // Sjekk med riktig passord - Skal være true
        bool riktig = student.SjekkPassord("hemmelig123");

        // Sjekk med feil passord - Skal være false
        bool feil = student.SjekkPassord("feilpassord");

        // Assert.IsTrue = "jeg forventer at dette er true"
        Assert.IsTrue(riktig);

        // Assert.IsFalse = "jeg forventer at dette er false"
        Assert.IsFalse(feil);
    }

    // TEST 2 - Tester at boken teller eksemplarer riktig
    [TestMethod]
    public void Bok_LanUtOgReturner()
    {
        // Lag en bok med 2 eksemplarer
        Bok bok = new Bok("B001", "C# med jenta", "Wadar Dose", 2022, 2);

        // Lån ut ett eksemplar
        bok.LanUt();

        // Assert.AreEqual betyr: "jeg forventer at disse to tallene er like"
        // 2 eksemplarer minus 1 utlånt = 1 ledig
        Assert.AreEqual(1, bok.Tilgjengelige());

        // Returner boken
        bok.Returner();

        // Nå skal det være 2 ledige igjen
        Assert.AreEqual(2, bok.Tilgjengelige());
    }

    // TEST 3 - Tester at påmelding og avmelding fungerer
    [TestMethod]
    public void Kurs_MeldPaOgMeldAv()
    {
        // Lag student og kurs
        Student student = new Student("S001", "Narceli", "test@uni.no", "passord");
        Kurs kurs = new Kurs("INF101", "Programmering", 10, 30, "F001");

        // Meld studenten på kurset
        kurs.MeldPa(student);

        // Sjekk at "INF101" nå ligger i studentens påmeldte kurs
        Assert.IsTrue(student.PamelteKurs.Contains("INF101"));

        // Meld studenten av kurset
        kurs.MeldAv(student);

        // Sjekk at "INF101" ikke lenger er i listen
        Assert.IsFalse(student.PamelteKurs.Contains("INF101"));
    }
}
