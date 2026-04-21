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

        // Riktig passord - Skal være true
        // Feil passord - Skal være false
        bool riktig = student.SjekkPassord("hemmelig123");
        bool feil = student.SjekkPassord("feilpassord");

        // Assert.IsTrue = "jeg forventer at dette er true"
        // Assert.IsFalse = "jeg forventer at dette er false"
        Assert.IsTrue(riktig);
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
        // Sjekk om studenten er påmeldt INF101
        kurs.MeldPa(student);
        Assert.IsTrue(student.PamelteKurs.Contains("INF101"));

        // Meld studenten av kurset
        // Sjekk at "INF101" ikke lenger er i listen
        kurs.MeldAv(student);
        Assert.IsFalse(student.PamelteKurs.Contains("INF101"));
    }
}
