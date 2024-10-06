using EightBitSaxLounge.Composer.Mxl.Models;
using EightBitSaxLounge.Composer.Mxl.Models.MusicTheory;
using EightBitSaxLounge.Composer.Mxl.Models.Xml;
using EightBitSaxLounge.Composer.Mxl.UnitTests.Models;

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

    [TestCase("Files/MeasureChordsC.xml", new [] { "Am (1)"})]
    [TestCase("Files/MeasureChordsBm.xml", new [] { "Bm (2)"})]
    public void DetermineChordsInMeasure_ShouldReturnExpectedResult(string measureFilepath, string[] expectedChords)
    {
        // Arrange
        var xmlMeasure = XmlTestHelper.CreateXmlElementFromFile(measureFilepath);
        var mxlMeasure = new MxlMeasure(xmlMeasure, XmlTestHelper.CreateTestMxlMeasureAttributes());

        // Act
        var chords = MeasureAnalyzer.DetermineChordsInMeasure(mxlMeasure);
        var relativeMinorChordsAnnotated = chords
            .Select(chord => ChordGenerator.AddChordLocation(chord, mxlMeasure.Key, true))
            .ToList();

        // Assert
        Assert.AreEqual(expectedChords.Length, relativeMinorChordsAnnotated.Count);
        for (var i = 0; i < expectedChords.Length; i++)
        {
            Assert.AreEqual(expectedChords[i], relativeMinorChordsAnnotated[i]);
        }
    }
}