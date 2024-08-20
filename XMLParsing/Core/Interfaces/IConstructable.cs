using XMLParsing.Core.Models;

namespace XMLParsing.Core.Interfaces
{
    public interface IConstructable<T>
    {
        List<T> Construct(string text);
    }
}
