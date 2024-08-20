using XMLParsing.Core;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;
using XMLParsing.Infrastructure.Constructors;
using XMLParsing.Infrastructure.Loggers;
using XMLParsing.Infrastructure.XmlParsing;
using XMLParsing.Infrastructure.XMLSource;


IXmlSource fileXmlSource = new FileXmlSourse();

ILogger logger = new ConsoleLogger();
IParse<Chapter> chapterParser = new ChapterParser(logger);
IParse<Book> bookParser = new BookParser(logger, chapterParser);
IParse<Member> memberParser = new MemberParser(logger);

IConstructable<Book> bookConstructor = new ConstructorBook(bookParser);
IConstructable<Member> memberConstructor = new ConstructorMember(memberParser);


Library library = new Library(fileXmlSource, bookConstructor, memberConstructor);
logger.Log("Welcome to Library!");


logger.Log($"\nTotal number of chapters: {library.TotalNumberOfChapters()}");

logger.Log($"\nBorrowed books id: ");
library.BorrowedBooksId().ForEach(id => logger.Log(id.ToString()));

logger.Log($"\nAll books sorted by genre: ");
library.BooksSortedByGenre().ForEach(book => logger.Log(book.ToString()));



Console.ReadLine();