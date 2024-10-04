using System.Reflection.Metadata;
using System.Xml.Linq;

namespace EightBitSaxLounge.Composer.Mxl.Models.Xml;

public static class XmlParser
{
    public static string GetValueFromElementListByLocationAndChildElementName(
        XmlElements parentElements, 
        int parentElementindex,
        string childElementName)
    {
        return parentElements.Elements.ElementAt(parentElementindex).Element(childElementName)?.Value ??
               throw new InvalidOperationException();
    }

    // public static string GetByAttribute(XmlElement element, string elementAttribute)
    // {
    //     return element.Element?.Element("attributes")?
    //     return xmlMeasure.Element?.Element("attributes")?.Element("key")?.Element("fifths")
    // }
    public static string? GetValueFromElement(XmlElement element)
    {
        return element.Element?.Value;
    }

    public static string? GetValueFromElementChildByName(XmlElement parentElement, string childName)
    {
        return parentElement.Element?.Element(childName)?.Value;
    }

    public static string? GetValueFromElementByAttribute(XmlElement element, string attribute)
    {
        return element.Element?.Attribute(attribute)?.Value!;
    }

    public static bool CheckForChildElement(XmlElement element, string childName)
    {
        return element.Element?.Element(childName) != null;
    }
}