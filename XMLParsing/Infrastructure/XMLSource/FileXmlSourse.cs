using XMLParsing.Core.Interfaces;

namespace XMLParsing.Infrastructure.XMLSource
{
    public class FileXmlSourse : IXmlSource
    {
        private readonly string fileName = @"/Users/oleksandr/RiderProjects/XmlParsing/doubled.txt";

        public string GetXmlFromSource()
        {
            return File.ReadAllText(fileName);
        }
    }
}
