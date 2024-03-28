using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace menu.Models
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("PID")]
        public int PID { get; set; }
        
        [Ignore]
        public List<UserList> UserLists { get; set; }
    }
}
