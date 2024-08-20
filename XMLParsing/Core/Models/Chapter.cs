namespace XMLParsing.Core.Models
{
    public class Chapter
    {
        public int Number { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }

        public Chapter(int number, string title, string content)
        {
            Number = number;
            Title = title;
            Content = content;
        }

        public override string ToString()
        {
            return $"\tNumber = {Number}\n" +
                $"\tTitle: \"{Title}\"\n" +
                $"\tContent: {Content}";
        }
    }
}
