using System.Runtime.CompilerServices;
using System.Xml.Linq;
using EightBitSaxLounge.Composer.Mxl.Models.Xml;

namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlDocument
{
    private XmlDocument Document { get; set; }
    public MxlScore Score { get; set; }

    public MxlDocument()
    {
        LoadFromFile();
        BuildScore();
    }
    
    public MxlDocument(string filePath)
    {
        Document = new XmlDocument(filePath);
        BuildScore();
    }
    
    public void LoadFromFile()
    {
        Console.WriteLine("Please enter the file path of the XML file:");
        string filePath = Console.ReadLine();
        try
        {
            Document = new XmlDocument(filePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void BuildScore()
    {
        Score = new MxlScore(Document);
    }
}