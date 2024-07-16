public interface IUserAction
{
    void AddUser(User user);
    List<User> GetUserList();
    void DeleteUser(string username);
    User GetUserByFilter(string username);
}