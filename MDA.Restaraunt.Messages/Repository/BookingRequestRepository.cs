using MDA.Restaraunt.Messages.DbData;
using Microsoft.EntityFrameworkCore;

namespace MDA.Restaraunt.Messages.Repository;

/// <summary>
/// Класс репозитория
/// </summary>
public class BookingRequestRequestRepository : IBookingRequestRepository
{
    private readonly AppDbContext _db;

    public BookingRequestRequestRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Добавить в базу данных
    /// </summary>
    /// <param name="entity">Объект</param>
    public async Task<bool> AddAsync(BookingRequestModel entity)
    {
        using (_db)
        {
            try
            {
                await _db.BookingRequestModels.AddAsync(entity);
                return await _db.SaveChangesAsync() >= 1;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Обновить модель в базе
    /// </summary>
    /// <param name="entity">Объект</param>
    public async Task<bool> UpdateAsync(BookingRequestModel entity)
    {
        using (_db)
        {
            try
            {
                _db.BookingRequestModels.Update(entity);
                return await _db.SaveChangesAsync() >= 1;
            }
            catch
            {
                return false;
            }
        }
    }


    /// <summary>
    /// Удаление из базы данных
    /// </summary>
    /// <param name="id">Id</param>
    public async Task<bool> DeleteByIdAsync(int id)
    {
        using (_db)
        {
            try
            {
                var item = await GetByIdAsync(id);
                _db.BookingRequestModels.Remove(item);
                return await _db.SaveChangesAsync() >= 1;
            }
            catch
            {
                return false;
            }
        }
            
    }
    /// <summary>
    /// Удаление из базы данных
    /// </summary>
    /// <param name="orderId">Id</param>
    public async Task<bool> DeleteByOrderIdAsync(Guid orderId)
    {
        using (_db)
        {
            try
            {
                var item = await GetByOrderIdAsync(orderId);
                _db.BookingRequestModels.Remove(item);
                return await _db.SaveChangesAsync() >= 1;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Выводит все объекты
    /// </summary>
    public async Task<IEnumerable<BookingRequestModel>> GetAllAsync()
    {
        using (_db)
        {
            return await _db.BookingRequestModels.ToListAsync();
        }
    }

    /// <summary>
    /// Выводит модель по Id
    /// </summary>
    /// <param name="id">Id</param>
    public async Task<BookingRequestModel> GetByIdAsync(int id)
    {
        using (_db)
        {
            return await _db.BookingRequestModels.
                FirstOrDefaultAsync(p => p.BookingRequestId == id);
        }
    }

    /// <summary>
    /// Выводит модель по Id
    /// </summary>
    /// <param name="orderId">Id</param>
    public async Task<BookingRequestModel> GetByOrderIdAsync(Guid orderId)
    {
        using (_db)
        {
            return await _db.BookingRequestModels.
                FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}