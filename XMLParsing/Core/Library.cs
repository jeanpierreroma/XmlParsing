using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;
using XMLParsing.Infrastructure.Constructors;

namespace XMLParsing.Core
{
    public class Library
    {
        public List<Book> Books { get; private set; }

        public List<Member> Members { get; private set; }
        public Library(
            IXmlSource xmlSource, 
            IConstructable<Book> bookConstructor, 
            IConstructable<Member> memberConstructor)
        {
            var xmlData = xmlSource.GetXmlFromSource();

            Books = bookConstructor.Construct(xmlData);
            Members = memberConstructor.Construct(xmlData);
        }

        // Get total chapters
        public int TotalNumberOfChapters()
        {
            return Books.Sum(book => book.Chapters.Length);
        }

        // Get all borrowed books
        public List<int> BorrowedBooksId()
        {
            return Members
                .SelectMany(member => member.BorrowedBooks.Keys)
                .ToList();
        }

        // Get all books stored by genre
        public List<Book> BooksSortedByGenre()
        {
            return Books.OrderBy(x => x.Genre).ToList();
        }
    }
}
