using SQLite;

namespace menu.Models
{
    [Table("list_items")]
    public class ListItem
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Indexed]
        [Column("list_id")]
        public int UserListId { get; set; }
        [MaxLength(100)]
        [Column("text")]
        public string Text { get; set; }
        [Column("is_complete")]
        public bool IsComplete { get; set; }
    }
}
