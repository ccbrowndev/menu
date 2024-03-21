using SQLite;

namespace menu.Models
{
    [Table("user_lists")]
    public class UserList
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }
        [Column("deadline")]
        public DateTime Deadline { get; set; }
        [Column("is_in_trash")]
        public bool IsInTrash { get; set; } = false;
        [Ignore]
        public List<ListItem> ListItems { get; set; }
    }
}
