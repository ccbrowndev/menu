using SQLite;

namespace menu.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("ID")]
        public int Id { get; set; }

        [Column("UUID")]
        public string Uuid { get; set; }

        [Ignore]
        public List<UserList> UserLists { get; set; }
    }
}
