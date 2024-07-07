namespace ProjectManagment.Models
{
    public class Board
    {
        public List<Column> Columns { get; set; }
        public Guid ProjectId { get; set; }
        public List<ProjectSprint> Sprints { get; set; }
        public Guid SelectedSprintId { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
    }

    public class Card
    {
        public Card(int number, Guid id, string title)
        {
            this.Number = number;
            this.id = id;
            this.Title = title;
        }

        public int Number { get; set; }
        public Guid id { get; set; }
        public string Title { get; set; }
    }

}
