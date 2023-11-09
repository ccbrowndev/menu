﻿using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace menu.Domain
{
    [SQLite.Table("DELETED_LIST_ITEM")]
    public class DeletedUserListItem
    {
        [PrimaryKey] public int id { get; set; }
        [ForeignKey("UserList")] public int UserListid { get; set; }
        public List<UserListItem> subItems { get; set; }
        public string text { get; set; }
        public bool completed { get; set; }
        public DateTime ddl { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}