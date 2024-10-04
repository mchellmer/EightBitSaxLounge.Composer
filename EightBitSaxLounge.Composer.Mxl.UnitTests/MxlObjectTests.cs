using System.Xml.Linq;
using EightBitSaxLounge.Composer.Mxl.Models;
using EightBitSaxLounge.Composer.Mxl.UnitTests.Models;
using NUnit.Framework;

namespace EightBitSaxLounge.Composer.Mxl.UnitTests
{
    public class Tests
    {
        private MxlDocument _mxlDocument;

        private MxlMetronome _mxlMetronomeTest = new (1)
        {
            Location = 0,
        };

        private MxlMeasureAttributes _mxlMeasureAttributesTest = new ()
        {
            BeatCount = 4,
            BeatType = 4,
            Key = 0,
        };

        [SetUp]
        public void Setup()
        {
            
        }
        
        [TestCase("Files/NotePitched.xml", "A", 4, 0, 24, null, 1, false)]
        [TestCase("Files/NoteSharp.xml", "A", 4, 1, 24, null, 1, false)]
        [TestCase("Files/NoteFlat.xml", "A", 4, -1, 24, null, 1, false)]
        [TestCase("Files/NoteAccidental.xml", "A", 4, 1, 24, "sharp", 1, false)]
        [TestCase("Files/NoteRest.xml", null, null, null, 24, null, 1, false)]
        public void MxlNoteConstructor_ShouldCreateCorrectMxlNote(string filePath, string? expectedStep, int? expectedOctave, int? expectedAlter, int expectedDuration, string? expectedAccidental, int expectedStaff, bool expectedIsChord)
        {
            // Arrange
            var xmlNote = XmlTestHelper.CreateXmlElementFromFile(filePath);

            // Act
            var result = new MxlNote(xmlNote);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Pitch?.Step, Is.EqualTo(expectedStep));
            Assert.That(result.Pitch?.Octave, Is.EqualTo(expectedOctave));
            Assert.That(result.Pitch?.Alter, Is.EqualTo(expectedAlter));
            Assert.That(result.Duration, Is.EqualTo(expectedDuration));
            Assert.That(result.Accidental, Is.EqualTo(expectedAccidental));
            Assert.That(result.Staff, Is.EqualTo(expectedStaff));
            Assert.That(result.IsChord, Is.EqualTo(expectedIsChord));
        }
        
        [TestCase("Files/MeasureDoubleStaff.xml", new [] {0}, new [] {0}, 4)]
        [TestCase("Files/MeasureDoubleStaffChord.xml", new [] {0, 0}, new [] {0}, 4)]
        [TestCase("Files/MeasureDoubleStaffChordTransition.xml", new [] {0, 0, 2}, new [] {0}, 4)]
        public void MxlMetronome_ShouldUpdateLocationCorrectly(
            string filePath,
            int[] noteLocationsStaff1,
            int[] noteLocationsStaff2,
            int finalLocation)
        {
            // Arrange
            var xmlMeasure = XmlTestHelper.CreateXmlElementFromFile(filePath);

            // Act
            var mxlMeasure = new MxlMeasure(xmlMeasure, _mxlMeasureAttributesTest);
            var actualNoteLocationsStaff1 = mxlMeasure.Notes
                .Where(note => note.Staff == 1)
                .Select(note => note.Location)
                .ToList();
            var actualNoteLocationsStaff2 = mxlMeasure.Notes
                .Where(note => note.Staff == 2)
                .Select(note => note.Location)
                .ToList();
            var actualFinalLocation1 = mxlMeasure.Notes
                .Where(note => note.Staff == 1)
                .Where(note => !note.IsChord)
                .Sum(note => note.Duration);
            var actualFinalLocation2 = mxlMeasure.Notes
                .Where(note => note.Staff == 2)
                .Where(note => !note.IsChord)
                .Sum(note => note.Duration);
            
            // Assert
            Assert.That(actualNoteLocationsStaff1, Is.EqualTo(noteLocationsStaff1));
            Assert.That(actualNoteLocationsStaff2, Is.EqualTo(noteLocationsStaff2));
            Assert.That(actualFinalLocation1, Is.EqualTo(finalLocation));
            Assert.That(actualFinalLocation2, Is.EqualTo(finalLocation));
        }
    }
}