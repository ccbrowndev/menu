namespace menu.Models
{
    public class ListItem
    {
        public int Id { get; set; }
        public int UserListId { get; set; }
        public string Text { get; set; }
        public bool IsComplete { get; set; }
    }
}
