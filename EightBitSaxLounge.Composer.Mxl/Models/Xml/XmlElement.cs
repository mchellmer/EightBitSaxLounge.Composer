using System.Xml.Linq;

namespace EightBitSaxLounge.Composer.Mxl.Models.Xml;

public class XmlElement
{
    public XElement? Element { get; set; }

    public XmlElement(XElement element)
    {
        Element = element;
    }
    public XmlElement(XDocument document, string rootElement)
    {
        Element = document.Element(rootElement);
    }
    public XmlElement(XmlElement parentElement, string childName)
    {
        Element = parentElement.Element?.Element(childName);
    }
}