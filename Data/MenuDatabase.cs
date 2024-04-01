using menu.Models;
using SQLite;

namespace menu.Data
{
    public class MenuDatabase
    {
        private SQLiteConnection db;

        private static string DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "menu.db");

        public MenuDatabase()
        {
            db = new SQLiteConnection(DatabasePath);
            db.CreateTable<UserList>();
            db.CreateTable<ListItem>();
            db.CreateTable<User>();
        }

        public User GetDefaultUser()
        {
            return db.Query<User>("SELECT * FROM user WHERE id = 0").FirstOrDefault();
        }

        public List<UserList> GetUserLists()
        {
            List<UserList> lists;

            lists = db.Query<UserList>("SELECT * FROM user_lists WHERE is_in_trash = 0");

            if (lists == null || lists.Count == 0)
            {
                db.Insert(defaultUser);

                db.Insert(defaultUserList);

                db.Insert(changeTitleItem);
                db.Insert(addListItemItem);
                db.Insert(addListItem);
                db.Insert(deleteListItemItem);

                lists = db.Query<UserList>("SELECT * FROM user_lists WHERE is_in_trash = 0");
            }

            return lists;
        }

        public List<UserList> GetTrashUserLists()
        {
            List<UserList> trashLists;

            trashLists = db.Query<UserList>("SELECT * FROM user_lists WHERE is_in_trash = 1");

            if (trashLists == null || trashLists.Count == 0)
            {
                db.Insert(defaultUser);

                db.Insert(defaultUserList);

                db.Insert(changeTitleItem);
                db.Insert(addListItemItem);
                db.Insert(addListItem);
                db.Insert(deleteListItemItem);

                trashLists = db.Query<UserList>("SELECT * FROM user_lists WHERE is_in_trash = 1");
            }

            return trashLists;
        }

        public int SaveUserList(UserList list)
        {
            if (list.Id != 0)
            {
                db.Update(list);
                return list.Id;
            }
            else
            {
                db.Insert(list);
                int newListId = db.Query<int>("SELECT seq FROM sqlite_sequence WHERE name=\"user_lists\"").First();
                return newListId;
            }
        }

        public void MoveToTrash(UserList list)
        {
            if (list.Id == 1)
            {
                db.Update(list);
            }
            if (list.Id != 1)
            {
                list.IsInTrash = true;
                db.Update(list);
            }
        }

        public void RestoreFromTrash(UserList list)
        {
            list.IsInTrash = false;
            db.Update(list);
        }

        public void DeleteUserListPermanently()
        {
            db.Execute("DELETE FROM user_lists WHERE is_in_trash = 1");
        }


        public List<ListItem> GetListItemsByListId(int id)
        {
            return db.Table<ListItem>().Where(li => li.UserListId == id).ToList();
        }

        public List<UserList> GetTrashLists()
        {
            return db.Table<UserList>().Where(li => li.IsInTrash == true).ToList();
        }

        public ListItem GetListItemById(int id)
        {
            return db.Table<ListItem>().Where(li => li.Id == id).FirstOrDefault();
        }

        public int SaveListItem(ListItem li)
        {
            if (li.Id != 0)
            {
                return db.Update(li);
            }
            else
            {
                return db.Insert(li);
            }
        }

        public ListItem DeleteListItem(ListItem li)
        {
            int result = db.Delete(li);
            if (result == 0)
            {
                return null;
            }
            else if (result == 1)
            {
                return li;
            }
            else
            {
                throw new Exception(string.Format("Error occurred trying to delete ListItem {0}", li));
            }
        }

        private readonly User defaultUser = new()
        {
            Id = 0,
            Uuid = Guid.NewGuid().ToString()
        };

        private readonly UserList defaultUserList = new()
        {
            Name = "Welcome",
        };

        private static readonly ListItem changeTitleItem = new()
        {
            UserListId = 1,
            Text = "Tap \"Welcome\" to change the list title.",
            IsComplete = false
        };

        private static readonly ListItem addListItemItem = new()
        {
            UserListId = 1,
            Text = "Type into the placeholder below to add items.",
            IsComplete = false
        };

        private static readonly ListItem addListItem = new()
        {
            UserListId = 1,
            Text = "Click the add button to create a new list.",
            IsComplete = false
        };

        private static readonly ListItem deleteListItemItem = new()
        {
            UserListId = 1,
            Text = "Swipe to the left on an item to reveal the delete button.",
            IsComplete = false
        };
    }
}
