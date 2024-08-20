namespace XMLParsing.Core.Interfaces
{
    public interface IParse<T> where T : class
    {
        T Parse(string text);
    }
}
