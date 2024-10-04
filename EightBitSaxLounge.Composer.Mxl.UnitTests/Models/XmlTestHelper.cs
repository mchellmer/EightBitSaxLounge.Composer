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

    public static XmlElements CreateXmlElementsFromFileAndRoot(string filePath, string root)
    {
        var xElements = XElement.Load(filePath).Elements(root);
        return new XmlElements(xElements);
    }
}