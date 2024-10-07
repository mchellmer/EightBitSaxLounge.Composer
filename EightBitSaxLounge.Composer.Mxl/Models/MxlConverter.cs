using System.Xml.Linq;
using EightBitSaxLounge.Composer.Mxl.Models.MusicTheory;
using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public static class MxlConverter
{
    public static XmlDocument ConvertMxlDocumentToXmlDocument(MxlDocument mxlDocument)
    {
        // Replace notes in mxlDocument.Document with notes in mxlDocument.Score
        ConvertXmlDocumentNotes(mxlDocument);
        
        return mxlDocument.Document;
    }

    private static void ConvertXmlDocumentNotes(MxlDocument mxlDocument)
    {
        var xmlParts = mxlDocument.Document.Document.Descendants("part").ToList();
        var scoreParts = mxlDocument.Score.Parts;

        if (xmlParts.Count != scoreParts.Count)
        {
            throw new InvalidOperationException("Mismatch between number of XML parts and score parts.");
        }

        for (int i = 0; i < scoreParts.Count; i++)
        {
            var xmlMeasures = xmlParts[i].Descendants("measure").ToList();
            var scoreMeasures = scoreParts[i].Measures;

            if (xmlMeasures.Count != scoreMeasures.Count)
            {
                throw new InvalidOperationException("Mismatch between number of XML measures and score measures.");
            }

            for (int j = 0; j < scoreMeasures.Count; j++)
            {
                var xmlNotes = xmlMeasures[j].Descendants("note").ToList();
                var scoreNotes = scoreMeasures[j].Notes;

                if (xmlNotes.Count != scoreNotes.Count)
                {
                    throw new InvalidOperationException("Mismatch between number of XML notes and score notes.");
                }

                for (int k = 0; k < scoreNotes.Count; k++)
                {
                    var xmlNote = xmlNotes[k];
                    var scoreNote = scoreNotes[k];

                    var xmlPitch = xmlNote.Element("pitch");
                    if (xmlPitch != null)
                    {
                        var scorePitch = scoreNote.Pitch;
                        UpdateXmlPitchToMatchScorePitch(xmlPitch, scorePitch);
                    }
                }
            }
        }
    }
    
    public static void UpdateXmlPitchToMatchScorePitch(XElement xmlPitch, MxlPitch scorePitch)
    {
        xmlPitch.Element("step")?.SetValue(scorePitch.Step);
        xmlPitch.Element("octave")?.SetValue(scorePitch.Octave);
        var alterElement = xmlPitch.Element("alter");
        if (alterElement == null && (scorePitch.Alter == -1 || scorePitch.Alter == 1))
        {
            alterElement = new XElement("alter", scorePitch.Alter);
            xmlPitch.Add(alterElement);
        }
        else
        {
            alterElement?.SetValue(scorePitch.Alter);
        }
    }

    public static void ConvertMxlNotesToMinor(MxlDocument mxlDocument)
    {
        foreach (var part in mxlDocument.Score.Parts)
        {
            foreach (var measure in part.Measures)
            {
                foreach (var note in measure.Notes)
                {
                    var pitch = note.Pitch;
                    if (pitch == null) continue; // Skip rest notes

                    note.Pitch = ConvertMxlPitchToRelativeMinor(pitch, measure.Key);
                }
            }
        }
    }

    public static MxlPitch ConvertMxlPitchToRelativeMinor(MxlPitch pitch, int keyAsFifths)
    {
        // Construct the note string (e.g., "C5" or "C#5" or "Cb5")
        var noteName = $"{pitch.Step}{(pitch.Alter == 1 ? "#" : pitch.Alter == -1 ? "b" : "")}{pitch.Octave}";

        // Transpose to relative minor
        var transposedNote = NoteConverter.TransposeNoteToRelativeMinor(noteName, keyAsFifths);

        // Parse the transposed note back into step, alter, and octave
        var newStep = transposedNote.Substring(0, transposedNote.Length - 1);
        var newOctave = int.Parse(transposedNote[^1].ToString());
        var newAlter = 0;

        if (newStep.Length > 1)
        {
            if (newStep[1] == '#')
            {
                newAlter = 1;
                newStep = newStep[0].ToString();
            }
            else if (newStep[1] == 'b')
            {
                newAlter = -1;
                newStep = newStep[0].ToString();
            }
        }
        
        pitch.Step = newStep;
        pitch.Alter = newAlter;
        pitch.Octave = newOctave;
        
        return pitch;
    }

    public static void AddChordAnnotationsToMeasures(MxlDocument mxlDocument)
    {
        var xmlDirectionChordAnnotation = new XmlElement("Files/MxlDirectionChordAnnotation.xml");
        var scoreParts = mxlDocument.Score.Parts;

        int measureIndex = 0;
        foreach (var part in scoreParts)
        {
            foreach (var mxlMeasure in part.Measures)
            {
                // Determine the chord value
                var chords = MeasureAnalyzer.DetermineChordsInMeasure(mxlMeasure);
                var chordsString = string.Join(" ",
                    chords.Select(chord => ChordGenerator.AddChordLocation(chord,
                        mxlMeasure.Key,
                        true)));

                XmlParser.UpdateElementValue(xmlDirectionChordAnnotation, "words", chordsString);

                // Insert the new direction element before the first note element
                XmlParser.AddChildElementToParentPreceedingSibling(
                    mxlDocument.Document,
                    xmlDirectionChordAnnotation,
                    "measure",
                    "number",
                    mxlMeasure.Number.ToString(),
                    "note");

                measureIndex++;
            }
        }
    }
}