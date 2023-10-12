using SQLite;

namespace menu.Domain
{
    [SQLite.Table("LIST_ITEM")]
    public class UserListItem
    {
        [PrimaryKey] public int id { get; set; }
        public List<UserListItem> subItems { get; set; }
        public string text;
    }
}