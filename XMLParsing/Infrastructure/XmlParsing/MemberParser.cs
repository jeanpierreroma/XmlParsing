using System.Text.RegularExpressions;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;

namespace XMLParsing.Infrastructure.XmlParsing
{
    public class MemberParser : IParse<Member>
    {
        private readonly ILogger _logger;

        // language=regex
        private readonly string _memberPattern =
            @"(Member id=""(?<id>\d+)"")[\s\S]*?" +
            @"(Name>(?<name>.+)<\/Name>)[\s\S]*?" +
            @"(MembershipDate>(?<membershipDate>.+)<\/MembershipDate)[\s\S]*?" +
            @"(?<borrowedBooks><BooksBorrowed[\s\S]*?<\/BooksBorrowed>)";

        // language=regex
        private readonly string _borrowedBook = 
            @"<Book id=""(?<id>\d+)"" dueDate=""(?<dueDate>\S+)"" />";

        public MemberParser(ILogger logger)
        {
            _logger = logger;
        }
        public Member Parse(string text)
        {
            var member = Regex.Match(text, _memberPattern);

            if (member.Success)
            {
                var booksBorrowed = Regex.Matches(member.Groups["borrowedBooks"].Value, _borrowedBook);

                Dictionary<int, DateTime> borrowedBooks = new Dictionary<int, DateTime>(booksBorrowed.Count);
                foreach (Match match in booksBorrowed.Cast<Match>())
                {
                    var singleBorrowedBook = Regex.Match(match.Value, _borrowedBook);

                    int id = int.Parse(singleBorrowedBook.Groups["id"].Value);

                    var date = singleBorrowedBook.Groups["dueDate"].Value.Split("-");
                    var newDate = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]));

                    borrowedBooks.Add(id, newDate);
                }

                var memberShipDate = member.Groups["membershipDate"].Value.Split("-");
                var newMemberShipDate = new DateTime(int.Parse(memberShipDate[0]), int.Parse(memberShipDate[1]), int.Parse(memberShipDate[2]));

                return new Member()
                {
                    Id = int.Parse(member.Groups["id"].Value),
                    Name = member.Groups["name"].Value,
                    MemberShipDate = newMemberShipDate,
                    BorrowedBooks = borrowedBooks
                };
            }
            else
            {
                _logger.Log("Error with parsing book!".ToUpperInvariant());

                return new Member()
                {
                    Id = -1,
                    Name = "Error",
                    MemberShipDate = new DateTime(1900, 01, 01),
                };
            }
        }
    }
}
