using System.Xml.Linq;

namespace EightBitSaxLounge.Composer.Mxl.Models.Xml;

public class XmlElements
{
    public IEnumerable<XElement> Elements { get; set; }

    public XmlElements(IEnumerable<XElement> xElements)
    {
        Elements = xElements;
    }
    public XmlElements(XmlElement parentElement, string childrenName)
    {
        Elements = parentElement.Element?.Elements(childrenName) ?? throw new InvalidOperationException();
    }
    public XmlElements(XmlElements parentElements, int parentIndex, string childrenName)
    {
        Elements = parentElements.Elements?.ElementAt(parentIndex)
                       .Elements(childrenName) ??
                   throw new InvalidOperationException();
    }
}