namespace AttractionReviewAPI.Repositories;

public class AttractionRepository : IAttractionRepository
{
    private readonly APIDBContext _context;
    
    public AttractionRepository(APIDBContext context)
    {
        _context = context;
    }

    //<summary>
    // получает все достопримечательности из базы данных
    //</summary>
    public IEnumerable<Attraction> GetAll() => _context.Attractions.ToList();
    
    // <summary>
    // получает достопримечательность по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор достопримечательности</param>
    public Attraction GetById(int id)
    {
        var attraction = _context.Attractions.FirstOrDefault(a => a.Id == id);
        return attraction;
    }

    // <summary>
    // создает новая достопримечательность в базе данных
    // </summary>
    // <param name="entity">объект достопримечательности для создания</param>
    public Attraction Create(Attraction entity)
    {
        _context.Attractions.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    // <summary>
    // обновляет существующую достопримечательность в базе данных
    // </summary>
    // <param name="entity">объект достопримечательности с обновленными данными</param>
    public Attraction Update(Attraction entity)
    {
        _context.Attractions.Update(entity);
        _context.SaveChanges();
        return entity;
    }

    // <summary>
    // удаляет достопримечательность по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор достопримечательности для удаления</param>
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
    
    // <summary>
    // проверяет существование достопримечательности по указанному идентификатору
    // </summary>
    // <param name="id">идентификатор достопримечательности</param>
    public bool Exists(int id) => _context.Attractions.Any(a => a.Id == id);

    
    // <summary>
    // проверяет существование достопримечательности по указанному названии
    // </summary>
    // <param name="id">название достопримечательности</param>
    public bool AttractionExist(string name) => _context.Attractions.Any(a => a.Name == name);
}