using System.Collections.Generic;
using menu.Domain;

namespace menu.Services
{
    public interface IMenuManager
    {
        int AddUser(User user);
        UserList AddUserList(UserList userList);
        void AddUserListItem(UserListItem userListItem);
    }
}
