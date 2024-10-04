using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlPitch
{
    public string? Step { get; set; }
    public int Octave { get; set; }
    public int Alter { get; set; }

    public MxlPitch(XmlElement xmlNote)
    {
        var pitchElement = new XmlElement(xmlNote, "pitch");
        Step = XmlParser.GetValueFromElementChildByName(pitchElement, "step");
        Octave = int.Parse(XmlParser.GetValueFromElementChildByName(pitchElement, "octave")!);
        Alter = XmlParser.CheckForChildElement(pitchElement,"alter")
            ? int.Parse(XmlParser.GetValueFromElementChildByName(pitchElement,
                "alter")!)
            : 0;
    }
}