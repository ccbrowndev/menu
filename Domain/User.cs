using SQLite;

namespace menu.Domain
{
    [SQLite.Table("USER")]
    public class User
    {
        [PrimaryKey] public int id {  get; set; }
        [SQLite.MaxLength(50)] public string name { get; set; }
    }
}
