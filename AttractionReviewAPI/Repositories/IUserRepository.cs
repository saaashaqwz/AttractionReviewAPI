namespace AttractionReviewAPI.Repositories;

public interface IUserRepository
{
    User GetUserById(int id);
    User AddUser(User user);
    User UpdateUser(int id, User user);
    bool DeleteUser(int id);
    User ExistUser(string emailOrUsername);
    Role? RoleExist(int id);
}