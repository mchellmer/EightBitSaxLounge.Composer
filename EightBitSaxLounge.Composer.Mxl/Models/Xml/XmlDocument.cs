using System.Xml.Linq;

namespace EightBitSaxLounge.Composer.Mxl.Models.Xml;

public class XmlDocument
{
    public XDocument Document { get; set; }

    public XmlDocument (string filepath)
    {
        Document = XDocument.Load(filepath);
    }
}