using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace menu.Domain
{
    [SQLite.Table("LIST_ITEM")]
    public class UserListItem
    {
        [PrimaryKey] public int id { get; set; }
        [ForeignKey("UserList")] public int UserListid { get; set; }
        public string text { get; set; }
        public bool completed { get; set; }
        public DateTime ddl { get; set; }
    }
}