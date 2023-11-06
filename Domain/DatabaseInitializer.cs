using SQLite;
using menu.Domain;

namespace menu.Services
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using (var db = SQLiteDb.GetConnection())
            {
                db.CreateTable<User>();
                db.CreateTable<UserList>();
                db.CreateTable<UserListItem>();
            }
        }
    }
}
