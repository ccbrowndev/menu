using SQLite;
using menu;
using menu.Domain;
using menu.Services;
using System.Collections.Generic;

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
                var deletedList = new DeletedUserList
                {
                    // Copy all relevant data from the 'list' to 'deletedList'
                    id = list.id,
                    Userid = list.Userid,
                    name = list.name,
                    listItems = list.listItems,
                    DeletedDate = DateTime.UtcNow // Set the deleted date to the current date/time
                };

                db.Insert(deletedList);
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
                    var deletedItem = new DeletedUserListItem
                    {
                        // Copy all relevant data from 'item' to 'deletedItem'
                        id = item.id,
                        UserListid = item.UserListid,
                        subItems = item.subItems,
                        text = item.text,
                        completed = item.completed,
                        ddl = item.ddl,
                        DeletedDate = DateTime.UtcNow // Set the deleted date to the current date/time
                    };

                    db.Insert(deletedItem);
                    db.Delete<UserListItem>(id);
                }
            }
        }

        // Empty recycle Bin
        public void EmptyListFromRecycleBin(int id)
        {
            var listItems = db.Table<UserListItem>().Where(li => li.UserListid == id).ToList();
            foreach (var item in listItems)
            {
                db.Delete(item);
            }

            var list = GetListByid(id);
            if(list != null)
            {
                db.Delete<DeletedUserList>(id);
            }
        }

        public void EmptyItemFromRecycleBin(int id)
        {
            var item = GetItemByid(id);
            if (item != null)
            {
                db.Delete<DeletedUserListItem>(id);
            }
        }

        // Attempt to retrieve the deleted list from the recycle bin
        public bool RestoreList(int id)
        {
            var deletedList = db.Table<DeletedUserList>().FirstOrDefault(l => l.id == id);

            if (deletedList != null)
            {
                // Create a new UserList instance and copy properties from the deleted list
                var restoredList = new UserList
                {
                    id = deletedList.id,
                    Userid = deletedList.Userid,
                    name = deletedList.name,
                    listItems = deletedList.listItems
                };

                db.Insert(restoredList);
                db.Delete<DeletedUserList>(id);

                return true; // Success
            }

            return false; // Failed to restore, list not found
        }


        // Attempt to retrieve the deleted item from the recycle bin
        public bool RestoreItem(int id)
        {
            var deletedItem = db.Table<DeletedUserListItem>().FirstOrDefault(i => i.id == id);

            if (deletedItem != null)
            {
                // Create a new UserListItem instance and copy properties from the deleted item
                var restoredItem = new UserListItem
                {
                    id = deletedItem.id,
                    UserListid = deletedItem.UserListid,
                    subItems = deletedItem.subItems,
                    text = deletedItem.text,
                    completed = deletedItem.completed,
                    ddl = deletedItem.ddl,
                };

                // Insert the restored item into the original UserListItem table
                db.Insert(restoredItem);

                // Remove the deleted item from the recycle bin
                db.Delete<DeletedUserListItem>(id);

                return true; // Success
            }

            return false; // Failed to restore, item not found
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
