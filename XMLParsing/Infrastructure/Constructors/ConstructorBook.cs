using System.Text.RegularExpressions;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;

namespace XMLParsing.Infrastructure.Constructors
{
    public class ConstructorBook : IConstructable<Book>
    {
        private readonly IParse<Book> _bookParser;

        // language=regex
        private readonly string _bookPattern = @"(<Book [\s\S]*?<\/Book>)";

        public ConstructorBook(IParse<Book> bookParser)
        {
            _bookParser = bookParser;
        }

        public List<Book> Construct(string text)
        {
            List<Book> result = new List<Book>();

            var xmlMatches = Regex.Matches(text, _bookPattern);

            foreach (Match bookMatch in xmlMatches.Cast<Match>())
            {
                var book = _bookParser.Parse(bookMatch.Value);
                result.Add(book);
            }

            return result;
        }
    }
}
