namespace menu.Model
{
    public class UserList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public List<UserListItem> ListItems { get; set; }
    }
}
