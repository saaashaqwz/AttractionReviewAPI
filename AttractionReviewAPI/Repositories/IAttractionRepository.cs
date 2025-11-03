namespace AttractionReviewAPI.Repositories;

public interface IAttractionRepository : IRepository<Attraction>
{
    bool AttractionExist(string name);
}