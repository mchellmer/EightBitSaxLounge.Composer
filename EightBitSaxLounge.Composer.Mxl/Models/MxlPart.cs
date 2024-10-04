using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlPart
{
    public string Name { get; set; }

    public string Id { get; set; }

    public List<MxlMeasure> Measures { get; set; }

    public MxlPart(XmlElements xmlPartlistParts, XmlElements xmlParts, int partLocation)
    {
        var xmlMeasures = new XmlElements(xmlParts, partLocation, "measure");
        
        Name = XmlParser.GetValueFromElementListByLocationAndChildElementName(
            xmlPartlistParts, 
            partLocation, 
            "part-name");
        Id = XmlParser.GetValueFromElementListByLocationAndChildElementName(
            xmlPartlistParts, 
            partLocation, 
            "id");
        Measures = BuildMeasures(xmlMeasures);
    }
    
    private static List<MxlMeasure> BuildMeasures(XmlElements xmlMeasures)
    {
        var mxlMeasures = new List<MxlMeasure>();
        var mxlMeasureAttributes = new MxlMeasureAttributes();
        foreach (var xmlMeasure in xmlMeasures.Elements)
        {
            var xmlMeasureElement = new XmlElement(xmlMeasure);
            mxlMeasureAttributes.Set(xmlMeasureElement);
            
            var mxlMeasure = new MxlMeasure(xmlMeasureElement, mxlMeasureAttributes);
            mxlMeasures.Add(mxlMeasure);
        }
        
        return mxlMeasures;
    }
}