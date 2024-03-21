using SQLite;
using CommunityToolkit.Mvvm.ComponentModel; //新

namespace menu.Models
{
    [Table("user_lists")]
    public class UserList : ObservableObject //新
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }
        [Column("deadline")]
        public DateTime Deadline { get; set; }

        // 新增属性：用于存储分享代码
        [MaxLength(50)] // 假定分享代码最大长度为10
        [Column("share_code")]
        public string ShareCode { get; set; }

        [Ignore]
        public List<ListItem> ListItems { get; set; }
    }
}
