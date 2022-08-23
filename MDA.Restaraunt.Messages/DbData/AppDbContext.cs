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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=./DbData/AppData.db");
        }
    }
}
