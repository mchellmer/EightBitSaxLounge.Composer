using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlMeasureAttributes
{
    public int Key { get; set; }
    public int BeatType { get; set; }
    public int BeatCount { get; set; }
    public int Divisions { get; set; }
    
    public void Set(XmlElement xmlMeasure)
    {
        var measureAttributesElement = new XmlElement(xmlMeasure, "attributes");
        
        var keyElement = new XmlElement(measureAttributesElement, "key");
        var fifthsValue = XmlParser.GetValueFromElementChildByName(keyElement, "fifths");
        Key = fifthsValue != null ? int.Parse(fifthsValue) : Key;

        var timeElement = new XmlElement(measureAttributesElement, "time");
        var beatTypeValue = XmlParser.GetValueFromElementChildByName(timeElement, "beat-type");
        BeatType = beatTypeValue != null ? int.Parse(beatTypeValue) : BeatType;

        var beatCountValue = XmlParser.GetValueFromElementChildByName(timeElement, "beats");
        BeatCount = beatCountValue != null ? int.Parse(beatCountValue) : BeatCount;
        
        var divisionsValue = XmlParser.GetValueFromElementChildByName(xmlMeasure, "divisions");
        Divisions = divisionsValue != null ? int.Parse(divisionsValue) : Divisions;
    }
}