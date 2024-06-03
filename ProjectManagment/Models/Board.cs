namespace ProjectManagment.Models
{
    public class Board
    {
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
    }

    public class Card
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
