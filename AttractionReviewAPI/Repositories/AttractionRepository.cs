namespace AttractionReviewAPI.Repositories;

public class AttractionRepository : IAttractionRepository
{
    private readonly APIDBContext _context;
    
    public AttractionRepository(APIDBContext context)
    {
        _context = context;
    }

    public IEnumerable<Attraction> GetAll() => _context.Attractions.ToList();
    
    public Attraction GetById(int id)
    {
        var attraction = _context.Attractions.FirstOrDefault(a => a.Id == id);
        return attraction;
    }

    public Attraction Create(Attraction entity)
    {
        _context.Attractions.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public Attraction Update(Attraction entity)
    {
        _context.Attractions.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public bool Delete(int id)
    {
        var attractions = GetById(id);
        if (attractions != null)
        {
            _context.Attractions.Remove(attractions);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
    public bool Exists(int id) => _context.Attractions.Any(a => a.Id == id);

    public bool AttractionExist(string name) => _context.Attractions.Any(a => a.Name == name);
}