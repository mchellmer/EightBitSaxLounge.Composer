using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlScore
{
    public List<MxlPart> Parts { get; set; }
    
    public MxlScore(XmlDocument document)
    {
        var xmlParts = new XmlElement(document.Document, "score-partwise");
        Parts = BuildParts(xmlParts);
    }

    private List<MxlPart> BuildParts(XmlElement xmlScore)
    {
        var scoreParts = new List<MxlPart>();

        var xmlPartlist = new XmlElement(xmlScore, "part-list");
        var xmlPartlistParts = new XmlElements(xmlPartlist, "score-part");
        var xmlParts = new XmlElements(xmlScore, "part");
        for (int i = 0; i < xmlParts.Elements.Count(); i++)
        {
            var mxlPart = new MxlPart(xmlPartlistParts, xmlParts, i);
            scoreParts.Add(mxlPart);
        }
        
        return scoreParts;
    }
}