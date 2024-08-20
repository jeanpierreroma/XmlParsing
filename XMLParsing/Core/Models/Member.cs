namespace XMLParsing.Core.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime MemberShipDate { get; set; }
        public Dictionary<int, DateTime> BorrowedBooks { get; set; } = new Dictionary<int, DateTime>();
    }
}
