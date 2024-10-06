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
    
    public static void AddChildElementToParentPreceedingSibling(
        XmlDocument documentToUpdate,
        XmlElement newChildElement,
        string parentName,
        string parentAttributeName,
        string parentAttributeValue,
        string siblingName)
    {
        var parentElements = documentToUpdate.Document.Descendants(parentName);
        foreach (var parentElement in parentElements)
        {
            if (parentElement.Attribute(parentAttributeName)?.Value == parentAttributeValue)
            {
                var siblingElements = parentElement.Elements(siblingName);
                if (siblingElements.Any())
                {
                    siblingElements.First().AddBeforeSelf(newChildElement.Element);
                }
                else
                {
                    parentElement.Add(newChildElement.Element);
                }
                break;
            }
        }
    }
    
    public static void UpdateElementValue(XmlElement element, string descendentToUpdate, string newValue)
    {
        var descendentElement = element.Element?.Descendants(descendentToUpdate).FirstOrDefault();
        if (descendentElement != null)
        {
            descendentElement.SetValue(newValue);
        }
    }

    public static bool CheckForChildElement(XmlElement element, string childName)
    {
        return element.Element?.Element(childName) != null;
    }

    public static void SaveXmlDocumentToFile(XmlDocument xmlDocument, string outputFilepath)
    {
        xmlDocument.Document.Save(outputFilepath);
    }
}