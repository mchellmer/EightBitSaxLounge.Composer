using EightBitSaxLounge.Composer.Mxl.Models.MusicTheory;

namespace EightBitSaxLounge.Composer.Mxl.UnitTests;

public class MusicTheoryTests
{
    [TestCase("C5", 0, true, "A4")]
    [TestCase("C5", 0, false, "A5")]
    [TestCase("C5", -5, true, "Ab4")]
    [TestCase("C5", -5, false, "Ab5")]
    [TestCase("A#5", 5, true, "F#5")]
    [TestCase("A#5", 5, false, "F#6")]
    [TestCase("D6", 0, true, "B5")]
    public void TransposeNoteToRelativeMinorDownward_ShouldReturnExpectedResult(string note, int keyAsFifths, bool descending, string expected)
    {
        var result = NoteConverter.TransposeNoteToRelativeMinor(note, keyAsFifths, descending);
        Assert.AreEqual(expected, result);
    }
}