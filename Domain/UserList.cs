using SQLite;

namespace menu.Domain
{
    [SQLite.Table("LIST")]
    internal class UserList
    {
        [PrimaryKey] public int id { get; set; }
        [SQLite.MaxLength(50)]public string name { get; set; }
        public List<UserListItem> listItems { get; set; }
    }
}
