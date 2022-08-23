using MDA.Restaraunt.Messages.DbData;

namespace MDA.Restaraunt.Messages.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _db;

    public Repository(AppDbContext db)
    {
        _db = db;
    }

    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Update(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Get()
    {
        throw new NotImplementedException();
    }

    public T GetByid(int id)
    {
        throw new NotImplementedException();
    }
}