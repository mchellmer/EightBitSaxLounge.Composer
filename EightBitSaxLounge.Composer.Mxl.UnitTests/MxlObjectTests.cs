using EightBitSaxLounge.Composer.Mxl.Models;
using EightBitSaxLounge.Composer.Mxl.Models.Xml;
using EightBitSaxLounge.Composer.Mxl.UnitTests.Models;

namespace EightBitSaxLounge.Composer.Mxl.UnitTests
{
    public class Tests
    {
        private static readonly MxlMeasureAttributes _mxlMeasureAttributesTest = new ()
        {
            BeatCount = 4,
            BeatType = 4,
            Key = 0,
            Divisions = 1
        };

        private static readonly XmlElements _xmlPartlistPartsTest = XmlTestHelper.CreatePartlistPartsTest();
        
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
        [TestCase("Files/MeasureDoubleStaffChordTransitionComplex.xml", new [] {0,0,0,0,1,1,1,2,3,3}, new [] {0,1,3}, 4)]
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

        [TestCase("Files/MeasuresStaticAttributes.xml",null, null)]
        [TestCase("Files/MeasuresDynamicAttributes.xml", new[] {4,4,0,2}, new[] {3,4,1,2})]
        public void MxlMeasureAttributes_ShouldBeSetCorrectly(
            string filePath,
            int[]? initialMeasureAttributesValues,
            int[]? transitionedMeasureAttributesValues)
        {
            // Arrange
            var xmlParts = XmlTestHelper.CreateXmlElementsFromFileAndRoot(filePath, "part");
            var expectedMeasuresAttributes = new List<MxlMeasureAttributes>
            {
                initialMeasureAttributesValues != null
                    ? new MxlMeasureAttributes
                    {
                        BeatCount = initialMeasureAttributesValues[0],
                        BeatType = initialMeasureAttributesValues[1],
                        Key = initialMeasureAttributesValues[2],
                        Divisions = initialMeasureAttributesValues[3]
                    }
                    : _mxlMeasureAttributesTest,
                transitionedMeasureAttributesValues != null
                    ? new MxlMeasureAttributes
                    {
                        BeatCount = transitionedMeasureAttributesValues[0],
                        BeatType = transitionedMeasureAttributesValues[1],
                        Key = transitionedMeasureAttributesValues[2],
                        Divisions = transitionedMeasureAttributesValues[3]
                    }
                    : _mxlMeasureAttributesTest
            };
            
            // Act
            var mxlPart = new MxlPart(_xmlPartlistPartsTest, xmlParts, 0);
            var mxlMeasuresAttributes = mxlPart.Measures.Select(measure => new MxlMeasureAttributes
            {
                Key = measure.Key,
                BeatType = measure.BeatType,
                BeatCount = measure.BeatCount,
                Divisions = measure.Divisions
            }).ToList();

            // Assert
            for (int i = 0; i < expectedMeasuresAttributes.Count; i++)
            {
                Assert.That(mxlMeasuresAttributes[i].BeatCount, Is.EqualTo(expectedMeasuresAttributes[i].BeatCount), $"BeatCount does not match at index {i}");
                Assert.That(mxlMeasuresAttributes[i].BeatType, Is.EqualTo(expectedMeasuresAttributes[i].BeatType), $"BeatType does not match at index {i}");
                Assert.That(mxlMeasuresAttributes[i].Key, Is.EqualTo(expectedMeasuresAttributes[i].Key), $"Key does not match at index {i}");
                Assert.That(mxlMeasuresAttributes[i].Divisions, Is.EqualTo(expectedMeasuresAttributes[i].Divisions), $"Divisions does not match at index {i}");
            }        
        }
        
        [TestCase("Files/MeasureDoubleStaff.xml", 1, 1, 1)]
        [TestCase("Files/MeasureDoubleStaffChord.xml", 1, 2, 1)]
        [TestCase("Files/MeasureDoubleStaffChordTransition.xml", 3, 3, 1)]
        [TestCase("Files/MeasureDoubleStaffChordTransitionComplex.xml", 1, 10, 3)]
        public void MxlMeasure_ShouldCreateCorrectMeasure(
            string filePath,
            int expectedMeasureNumberAttribute,
            int expectedMeasureNoteCountStaff1,
            int expectedMeasureNoteCountStaff2)
        {
            // Arrange
            var xmlMeasure = XmlTestHelper.CreateXmlElementFromFile(filePath);
            
            // Act
            var mxlMeasure = new MxlMeasure(xmlMeasure, _mxlMeasureAttributesTest);

            // Assert
            Assert.That(mxlMeasure.Number, Is.EqualTo(expectedMeasureNumberAttribute));
            Assert.That(mxlMeasure.Notes.Count(note => note.Staff == 1), Is.EqualTo(expectedMeasureNoteCountStaff1));
            Assert.That(mxlMeasure.Notes.Count(note => note.Staff == 2), Is.EqualTo(expectedMeasureNoteCountStaff2));
        }
        
        [TestCase("Files/Parts.xml", "Piano", "P1", 2)]
        public void MxlPart_ShouldCreateCorrectPart(
            string filePath,
            string expectedPartName,
            string expectedPartId,
            int expectedMeasureCount)
        {
            // Arrange
            var xmlPartlistParts = XmlTestHelper.CreatePartlistPartsFromFile(filePath);
            var xmlParts = XmlTestHelper.CreateXmlElementsFromFileAndRoot(filePath, "part");
            
            // Act
            var mxlPart = new MxlPart(xmlPartlistParts, xmlParts, 0);

            // Assert
            Assert.That(mxlPart.Name, Is.EqualTo(expectedPartName));
            Assert.That(mxlPart.Id, Is.EqualTo(expectedPartId));
            Assert.That(mxlPart.Measures.Count, Is.EqualTo(expectedMeasureCount));
        }
        
        [TestCase("Files/Score.xml", 1, 2, 10)]
        public void MxlScore_ShouldCreateCorrectScore(
            string filePath,
            int expectedPartCount,
            int expectedMeasureCount,
            int expectedNoteCount)
        {
            // Arrange
            var xmlDocument = new XmlDocument(filePath);
            
            // Act
            var mxlScore = new MxlScore(xmlDocument);
            
            // Assert
            Assert.That(mxlScore.Parts.Count, Is.EqualTo(expectedPartCount));
            Assert.That(mxlScore.Parts.SelectMany(part => part.Measures).Count(), Is.EqualTo(expectedMeasureCount));
            Assert.That(mxlScore.Parts.SelectMany(part => part.Measures.SelectMany(measure => measure.Notes)).Count(), Is.EqualTo(expectedNoteCount));
        }
    }
}