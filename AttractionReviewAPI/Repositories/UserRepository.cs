using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly APIDBContext _context;
    
    public UserRepository(APIDBContext context)
    {
        _context = context;
    }
    
    public User GetUserById(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
            return user;
        else return null;
    }
        
    public User AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
    
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
    
    public User ExistUser(string email)
    {
        var user = _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => 
                u.Email == email);

        return user;
    }
        
    public Role? RoleExist(int id)
    {
        return _context.Roles.FirstOrDefault(r => r.Id == id);
    }
}