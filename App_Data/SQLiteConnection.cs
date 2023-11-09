using SQLite;
using System;
using System.IO;

namespace menu
{
    public static class SQLiteDb
    {
        public static SQLiteConnection GetConnection()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MenuLocalDatabase.db3");
            var db = new SQLiteConnection(dbPath);

            return db;
        }
    }
}
