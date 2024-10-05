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
                        xmlPitch.Element("step")?.SetValue(scorePitch.Step);
                        xmlPitch.Element("alter")?.SetValue(scorePitch.Alter);
                        xmlPitch.Element("octave")?.SetValue(scorePitch.Octave);
                    }
                }
            }
        }
    }
}