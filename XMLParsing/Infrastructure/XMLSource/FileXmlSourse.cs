using XMLParsing.Core.Interfaces;

namespace XMLParsing.Infrastructure.XMLSource
{
    public class FileXmlSourse : IXmlSource
    {
        private readonly string fileName = "inputXML.txt";

        public string GetXmlFromSource()
        {
            return File.ReadAllText(fileName);
        }
    }
}
