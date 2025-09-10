using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using XMLParsing.Core;
using XMLParsing.Core.Interfaces;
using XMLParsing.Core.Models;
using XMLParsing.Infrastructure.Constructors;
using XMLParsing.Infrastructure.Loggers;
using XMLParsing.Infrastructure.XmlParsing;
using XMLParsing.Infrastructure.XMLSource;

namespace XMLParsing.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
[Config(typeof(BenchConfig))]
public class LibraryBench
{
    private ILogger _logger = default!;
    private IParse<Chapter> _chapterParser = default!;
    private IParse<Book> _bookParser = default!;
    private IParse<Member> _memberParser = default!;
    private IConstructable<Book> _bookConstructor = default!;
    private IConstructable<Member> _memberConstructor = default!;
    private IXmlSource _fileSource = default!;
    private Library _library = default!;
    
    [GlobalSetup(Target = nameof(Constructor_Library_FromFile))]
    public void SetupForCtor()
    {
        _logger = new ConsoleLogger();                      // твій логер
        _chapterParser = new ChapterParser(_logger);        // твої парсери
        _bookParser = new BookParser(_logger, _chapterParser);
        _memberParser = new MemberParser(_logger);

        _bookConstructor = new ConstructorBook(_bookParser);// твої конструктори
        _memberConstructor = new ConstructorMember(_memberParser);

        _fileSource = new FileXmlSourse();          // твій FileXmlSource
    }

    // --- Setup для бенчів методів (попередньо будуємо Library ОДИН раз)
    [GlobalSetup(Targets = new[] { nameof(TotalChapters), nameof(BorrowedIdsCount), nameof(SortedByGenre) })]
    public void SetupForQueries()
    {
        SetupForCtor();
        _library = new Library(_fileSource, _bookConstructor, _memberConstructor);
    }

    // 1) Вартість створення Library + читання/парсинг файлу
    [Benchmark(Description = "Library(..) from FileXmlSource")]
    public Library Constructor_Library_FromFile()
        => new Library(_fileSource, _bookConstructor, _memberConstructor);

    // 2) К-сть розділів
    [Benchmark(Description = "TotalNumberOfChapters()")]
    public int TotalChapters() => _library.TotalNumberOfChapters();

    // 3) К-сть позичених ідентифікаторів
    [Benchmark(Description = "BorrowedBooksId().Count")]
    public int BorrowedIdsCount() => _library.BorrowedBooksId().Count;

    // 4) Сортування за жанром
    [Benchmark(Description = "BooksSortedByGenre()")]
    public List<Book> SortedByGenre() => _library.BooksSortedByGenre();
}

public sealed class BenchConfig : ManualConfig
{
    public BenchConfig()
    {
        // Будуть середнє та P95
        AddColumn(TargetMethodColumn.Method, StatisticColumn.Mean, StatisticColumn.P95);

        // А це увімкне колонки пам’яті: Allocated, Gen0/1/2 тощо
        AddDiagnoser(MemoryDiagnoser.Default);
    }
}