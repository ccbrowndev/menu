﻿using menu.Models;
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
        }

        public List<UserList> GetUserLists()
        {
            List<UserList> lists = db.Query<UserList>("SELECT * FROM user_lists");

            if (lists == null || lists.Count == 0)
            {
                db.Insert(defaultUserList);

                db.Insert(changeTitleItem);
                db.Insert(addListItemItem);
                db.Insert(addListItem);
                db.Insert(deleteListItemItem);
                lists = db.Query<UserList>("SELECT * FROM user_lists");
            }

            return lists;
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

        public UserList DeleteUserList(UserList list)
        {
            int result = db.Delete(list);
            if (result == 0)
            {
                return null;
            } else if (result == 1)
            {
                return list;
            } else
            {
                throw new Exception(string.Format("Error occurred trying to delete UserList {0}", list));
            }
        }


        public List<ListItem> GetListItemsByListId(int id)
        {
            return db.Table<ListItem>().Where(li => li.UserListId == id).ToList();
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
            } else
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
            } else if (result == 1)
            {
                return li;
            } else
            {
                throw new Exception(string.Format("Error occurred trying to delete ListItem {0}", li));
            }
        }

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
