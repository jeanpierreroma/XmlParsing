namespace XMLParsing.Core.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Genre { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int PublicationDate { get; set; }
        public Chapter[] Chapters { get; set; } = null!;

        public override string ToString()
        {
            return $"ID: {Id}\n" +
              $"Genre: {Genre}\n" +
              $"Title: {Title}\n" +
              $"Author: {Author}\n" +
              $"Publication Date: {PublicationDate}\n" +
              $"Chapters:\n{string.Join("\n", Chapters.Select(c => c.ToString()))}";
        }
    }
}
