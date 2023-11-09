using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace menu.Domain
{
    [SQLite.Table("DELETED_LIST")]
    public class DeletedUserList
    {
        [PrimaryKey] public int id { get; set; }
        [ForeignKey("User")] public int Userid { get; set; }
        [SQLite.MaxLength(50)] public string name { get; set; }
        public List<UserListItem> listItems { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
