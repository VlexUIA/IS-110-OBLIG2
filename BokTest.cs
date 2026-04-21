using IS110OBLIG1;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IS110OBLIG1.Tests;

[TestClass]
[TestSubject(typeof(Bok))]
public class BokTest
{
    [TestMethod]
    public void SjekkPassord_RiktigOgFeilPassord()
    {
        Student student = new Student("S001", "Narceli", "test@uni.no", "hemmelig123");

        bool riktig = student.SjekkPassord("hemmelig123");
        bool feil   = student.SjekkPassord("feilpassord");

        Assert.IsTrue(riktig);
        Assert.IsFalse(feil);
    }

    [TestMethod]
    public void Bok_LanUtOgReturner()
    {
        Bok bok = new Bok("B001", "C# med jenta", "Wadar Dose", 2022, 2);

        bok.LanUt();
        Assert.AreEqual(1, bok.Tilgjengelige());

        bok.Returner();
        Assert.AreEqual(2, bok.Tilgjengelige());
    }

    [TestMethod]
    public void Kurs_MeldPaOgMeldAv()
    {
        Student student = new Student("S001", "Narceli", "test@uni.no", "passord");
        Kurs    kurs    = new Kurs("INF101", "Programmering", 10, 30, "F001");

        kurs.MeldPa(student);
        Assert.IsTrue(student.PamelteKurs.Contains("INF101"));

        kurs.MeldAv(student);
        Assert.IsFalse(student.PamelteKurs.Contains("INF101"));
    }
}
