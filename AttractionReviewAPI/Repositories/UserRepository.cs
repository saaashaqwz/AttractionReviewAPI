using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly APIDBContext _context;
    
    public UserRepository(APIDBContext context)
    {
        _context = context;
    }
    
    // <summary>
    // получает пользователя по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор пользователя</param>
    public User GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
            return user;
        else return null;
    }
        
    // <summary>
    // создает нового пользователя в базе данных
    // </summary>
    // <param name="user">объект пользователя</param>
    public User AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
    
    // <summary>
    // обновляет существующего пользователя в базе данных
    // </summary>
    // <param name="id">идентификатор пользователя</param>
    // <param name="user">объект пользователя</param>
    public User UpdateUser(int id, User user)
    {
        var existingUser = _context.Users.FirstOrDefault(u => u.Id == id);
        if (existingUser != null)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        return user;
    }
    
    // <summary>
    // удаляет пользователя по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор пользователя для удаления</param>
    public bool DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
        else return false;

    }
    
    // <summary>
    // проверяет существование пользователя по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор пользователя</param>
    public User ExistUser(string emailOrUsername)
    {
        var user = _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => 
                u.Email == emailOrUsername 
                || u.Username == emailOrUsername);

        return user;
    }
      
    // <summary>
    // проверяет существование роли по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор роли</param>
    public Role? RoleExist(int id)
    {
        return _context.Roles.FirstOrDefault(r => r.Id == id);
    }
}