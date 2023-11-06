using SQLite;
using menu;
using menu.Domain;
using menu.Services;

namespace UserSchedule.Services
{
    public class MenuManager : IMenuManager
    {
        /* Initialize database connection */

        private readonly SQLiteConnection db;

        public MenuManager()
        {
            db = SQLiteDb.GetConnection();
        }

        /* Add new Users or lists or Items */

        // Add a new User
        public int AddUser(User user)
        {
            int currentCount = db.Table<User>().Count();
            user.id = currentCount + 1;
            db.Insert(user);
            return user.id;
        }

        // Add a new List
        public void AddUserList(UserList userList)
        {
            db.Insert(userList);
        }

        // Add a new Item
        public void AddUserListItem(UserListItem userListItem)
        {
            db.Insert(userListItem);
        }

        // Upload the changes made to the local database (SQLite)
        public void SaveChanges()
        {
            db.Commit();
        }


        /* Delete Lists or Items */

        // Delete an existing List
        public void DeleteList(int id)
        {
            var list = GetListByid(id);
            if (list != null)
            {
                db.Delete<UserList>(id);
            }
        }

        // Delete an existing Item
        public void DeleteItem(int id)
        {
            var list = GetListByid(id);
            if (list != null)
            {
                var item = GetItemByid(id);
                if(item != null)
                {
                    db.Delete<UserListItem>(id);
                }
            }
        }

        /* Seek for Lists or Items*/

        // Seek for Lists
        public UserList GetListByid(int listid)
        {
            var list = db.Table<UserList>().FirstOrDefault(u => u.id == listid);
            return list;
        }

        // Seek for items
        public UserListItem GetItemByid(int itemid)
        {
            var item = db.Table<UserListItem>().FirstOrDefault(u => u.id == itemid);
            return item;
        }


    }
}
