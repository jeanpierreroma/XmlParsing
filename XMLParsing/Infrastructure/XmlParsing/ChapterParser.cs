using System.Text.RegularExpressions;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;

namespace XMLParsing.Infrastructure.XmlParsing
{
    public class ChapterParser : IParse<Chapter>
    {
        private readonly ILogger _logger;

        // language=regex
        private readonly string _chapterInformationPattern =
            @"(number=""(?<chapterNumber>\d+)"")[\s\S]*?" +
            @"(<Title>(?<chapterTitle>.+)<\/Title>)[\s\S]*?" +
            @"(<Content>(?<chapterContent>.+)<\/Content>)";

        public ChapterParser(ILogger logger)
        {
            _logger = logger;
        }
        public Chapter Parse(string text)
        {
            var chapterInformation = Regex.Match(text, _chapterInformationPattern);

            if (chapterInformation.Success)
            {
                int chapterNumber = int.Parse(chapterInformation.Groups["chapterNumber"].Value);
                string chapterTitle = chapterInformation.Groups["chapterTitle"].Value;
                string chapterContent = chapterInformation.Groups["chapterContent"].Value;

                return new Chapter(chapterNumber, chapterTitle, chapterContent);
            }
            else
            {
                _logger.Log("Error parsing with chapter!".ToUpperInvariant());

                return new Chapter(-1, "Error title", "Error Content");
            }

        }
    }
}
