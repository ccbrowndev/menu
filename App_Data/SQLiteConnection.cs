using SQLite;
using System;
using System.IO;

namespace menu
{
    public static class SQLiteDb
    {
        public static SQLiteConnection GetConnection()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dbPath = Path.Combine(folderPath, "menu", "App_Data", "ListManager.db");

            return new SQLiteConnection(dbPath);
        }
    }
}
