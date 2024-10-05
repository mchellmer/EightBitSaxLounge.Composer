using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlNote
{
    public MxlPitch? Pitch { get; set; }
    public int Duration { get; set; }
    public string? Accidental { get; set; }
    public int Staff { get; set; }
    public bool IsChord { get; set; }
    public int Location { get; set; }
    
    public MxlNote (XmlElement xmlNote)
    {
        if (XmlParser.CheckForChildElement(xmlNote, "pitch"))
        {
            Pitch = new MxlPitch(xmlNote);
            Accidental = XmlParser.GetValueFromElementChildByName(xmlNote, "accidental");
            IsChord = XmlParser.CheckForChildElement(xmlNote, "chord");
        }
        Duration = int.Parse(s: xmlNote.Element?.Element("duration")?.Value ?? throw new InvalidOperationException());
        Staff = int.Parse(xmlNote.Element?.Element("staff")?.Value ?? throw new InvalidOperationException());
    }
}