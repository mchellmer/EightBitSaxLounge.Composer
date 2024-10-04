using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlMeasure
{
    public int Number { get; set; }
    public int Key { get; set; }
    public int BeatType { get; set; }
    public int BeatCount { get; set; }
    public List<MxlNote> Notes { get; set; }

    public MxlMeasure(XmlElement xmlMeasure, MxlMeasureAttributes measureAttributes)
    {
        Number = int.Parse(XmlParser.GetValueFromElementByAttribute(xmlMeasure,
                               "number") ??
                           throw new InvalidOperationException());
        Key = measureAttributes.Key;
        BeatType = measureAttributes.BeatType;
        BeatCount = measureAttributes.BeatCount;
        Notes = BuildMeasureNotes(xmlMeasure, measureAttributes);
    }
    
    private List<MxlNote> BuildMeasureNotes(XmlElement xmlMeasure, MxlMeasureAttributes mxlMeasureAttributes)
    {
        var xmlMeasureNotes = new XmlElements(xmlMeasure, "note");
        var mxlMeasureNotes = new List<MxlNote>();
        
        // Initialize the first staff metronome
        var staffMetronomes = new List<MxlMetronome>();
        var staffMetronome = new MxlMetronome(1);
        staffMetronomes.Add(staffMetronome);
        
        foreach (var xmlMeasureNote in xmlMeasureNotes.Elements)
        {
            var xmlMeasureNoteElement = new XmlElement(xmlMeasureNote);
            // Build an mxl note and retrieve the metronome for the note's staff - create as staff count increases
            var mxlNote = new MxlNote(xmlMeasureNoteElement);
            staffMetronome = staffMetronomes.Find(m => m.Staff == mxlNote.Staff);
            if (staffMetronome == null)
            {
                staffMetronome = new MxlMetronome(mxlNote.Staff);
                staffMetronomes.Add(staffMetronome);
            }
            
            // Move the metronome location based on note size and measure attributes
            // - a note in a cord starts it's duration at the same time as the last note
            // - a note following a chord must adjust duration to account for the chord
            if (mxlNote.IsChord)
            {
                mxlNote.Location = staffMetronome.LastLocation;
                staffMetronome.Location = staffMetronome.LastLocation;
                staffMetronome.ChordTransitionDuration = mxlNote.Duration;
            }
            else
            {
                mxlNote.Location = staffMetronome.Location  + staffMetronome.ChordTransitionDuration;
                staffMetronome.LastLocation = staffMetronome.Location;
                staffMetronome.Location += mxlNote.Duration;
                staffMetronome.ChordTransitionDuration = 0;
            }
            
            mxlMeasureNotes.Add(mxlNote);
        }

        return mxlMeasureNotes;
    }
}