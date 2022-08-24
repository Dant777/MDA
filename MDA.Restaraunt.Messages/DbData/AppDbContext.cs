using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDA.Restaraunt.Messages.Repository;
using Microsoft.EntityFrameworkCore;

namespace MDA.Restaraunt.Messages.DbData
{
    public class AppDbContext : DbContext
    {
        public DbSet<BookingRequestModel> BookingRequestModels { get; set; }

        public AppDbContext(DbContextOptions options)
        : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = @"D:\YandexDisk\Обучение\GeekBrain\C#_U_MDA\MDA\MDA.Restaraunt.Messages\DbData\AppData.db";
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
