using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace menu.Domain
{
    [SQLite.Table("LIST")]
    public class UserList
    {
        [PrimaryKey] public int id { get; set; }
        [ForeignKey("User")] public int Userid { get; set; }
        [SQLite.MaxLength(50)]public string name { get; set; }
    }
}
