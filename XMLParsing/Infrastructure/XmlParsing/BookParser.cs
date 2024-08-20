using System.Text.RegularExpressions;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;

namespace XMLParsing.Infrastructure.XmlParsing
{
    public class BookParser : IParse<Book>
    {
        private readonly ILogger _logger;
        private readonly IParse<Chapter> _chapterParser;

        // language=regex
        private readonly string _bookDescriptionPattern = @"(id=""(?<id>\d+)"").*?" +
                                    @"(genre=""(?<genre>.+)"")[\s\S]*?" +
                                    @"(<Title>(?<title>.*)</Title>)[\s\S]*?" +
                                    @"(<Author>(?<author>.*)</Author>)[\s\S]*?" +
                                    @"(<PublicationDate>(?<publicationDate>.*)</PublicationDate>)[\s\S]*?" +
                                    @"(?<chapters><Chapters[\s\S]*?<\/Chapters>)";

        // language=regex
        private readonly string _singleChapter = @"(?<chapter><Chapter [\s\S]*?<\/Chapter>)";

        public BookParser(ILogger logger, IParse<Chapter> chapterParser)
        {
            _logger = logger;
            _chapterParser = chapterParser;
        }
        public Book Parse(string text)
        {
            var bookInformation = Regex.Match(text, _bookDescriptionPattern);

            if (bookInformation.Success)
            {
                var bookChapters = Regex.Matches(bookInformation.Groups["chapters"].Value, _singleChapter);

                Chapter[] chapters = new Chapter[bookChapters.Count];

                for (int i = 0; i < bookChapters.Count; i++)
                {
                    chapters[i] = _chapterParser.Parse(bookChapters[i].Value);
                }

                return new Book() 
                {
                    Id = int.Parse(bookInformation.Groups["id"].Value),
                    Genre = bookInformation.Groups["genre"].Value,
                    Title = bookInformation.Groups["title"].Value,
                    Author = bookInformation.Groups["author"].Value,
                    PublicationDate = int.Parse(bookInformation.Groups["publicationDate"].Value),
                    Chapters = chapters
                };
            }
            else
            {
                _logger.Log("Error with parsing book!".ToUpperInvariant());

                return new Book()
                {
                    Id = -1,
                    Genre = "Error",
                    Title = "Error",
                    Author = "Error",
                    PublicationDate = -1,
                    Chapters = new Chapter[1] { new Chapter(-1, "Error title", "Error Content") }
                };
            }
        }
    }
}
