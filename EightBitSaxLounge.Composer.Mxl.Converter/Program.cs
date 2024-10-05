using EightBitSaxLounge.Composer.Mxl.Models;
using EightBitSaxLounge.Composer.Mxl.Models.Xml;

// Load mxl file as MxlDocument
var mxlDocument = new MxlDocument();
MxlConverter.ConvertMxlNotesToMinor(mxlDocument);

// Convert MxlDocument to XmlDocument
var xmlDocument = MxlConverter.ConvertMxlDocumentToXmlDocument(mxlDocument);

// Save converted xmlDocument to file
XmlParser.SaveXmlDocumentToFile(xmlDocument, "output.xml");