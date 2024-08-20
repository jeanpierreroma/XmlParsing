using System.Text.RegularExpressions;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;

namespace XMLParsing.Infrastructure.Constructors
{
    public class ConstructorMember : IConstructable<Member>
    {
        private readonly IParse<Member> _memberParser;

        // language=regex
        private readonly string _memberPattern = @"(<Member [\s\S]*?<\/Member>)";

        public ConstructorMember(IParse<Member> memberParser)
        {
            _memberParser = memberParser;
        }
        public List<Member> Construct(string text)
        {
            List<Member> result = new List<Member>();

            var xmlMatches = Regex.Matches(text, _memberPattern);

            foreach (Match match in xmlMatches.Cast<Match>())
            {
                var member = _memberParser.Parse(match.Value);
                result.Add(member);
            }

            return result;
        }
    }
}
