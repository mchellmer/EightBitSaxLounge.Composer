using System.Reflection.Metadata;
using System.Xml.Linq;

namespace EightBitSaxLounge.Composer.Mxl.Models.Xml;

public static class XmlParser
{
    public static string GetValueFromElementListByLocationAndChildAttribute(
        XmlElements parentElements, 
        int parentElementindex,
        string childAttribute)
    {
        return parentElements.Elements.ElementAt(parentElementindex).Attribute(childAttribute)?.Value ??
               throw new InvalidOperationException();
    }
    
    public static string GetValueFromElementListByLocationChildName(
        XmlElements parentElements, 
        int parentElementindex,
        string childName)
    {
        return parentElements.Elements.ElementAt(parentElementindex).Element(childName)?.Value ??
               throw new InvalidOperationException();
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