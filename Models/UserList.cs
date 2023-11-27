namespace menu.Models
{
    public class UserList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ListItem> ListItems { get; set; }
    }
}
