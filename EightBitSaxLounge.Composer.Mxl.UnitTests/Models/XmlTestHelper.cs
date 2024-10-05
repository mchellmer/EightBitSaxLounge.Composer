using System.Xml.Linq;
using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.UnitTests.Models;

public static class XmlTestHelper
{
    public static XmlElement CreateXmlElementFromFile(string filePath)
    {
        var xElement = XElement.Load(filePath);
        return new XmlElement(xElement);
    }

    public static XmlElements CreateXmlElementsFromFileAndRoot(string filePath, string childrenName)
    {
        var xElements = XElement.Load(filePath).Elements(childrenName);
        return new XmlElements(xElements);
    }

    public static XmlElements CreatePartlistPartsTest()
    {
        var xElementPartListPart = new XElement("score-part",
            new XElement("part-name", "Piano"));
        xElementPartListPart.SetAttributeValue("id", "P1");
        var xmlElementsPartListParts = new XmlElements(new List<XElement>{xElementPartListPart});
        
        return xmlElementsPartListParts;
    }

    public static XmlElements CreatePartlistPartsFromFile(string filePath)
    {
        var scoreDocument = XElement.Load(filePath);
        var xmlPartlistParts = scoreDocument.Element("part-list")?.Elements("score-part");
        return new XmlElements(xmlPartlistParts ?? throw new InvalidOperationException());
    }
}