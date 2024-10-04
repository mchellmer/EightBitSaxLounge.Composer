using EightBitSaxLounge.Composer.Mxl.Models;

// Load mxl file as XDocument
var mxlDocument = new MxlDocument();
mxlDocument.LoadFromFile();
mxlDocument.BuildScore();
