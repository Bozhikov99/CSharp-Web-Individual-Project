using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<CinemaDbContext> options;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("FileName=:memory:");
            connection.Open();

            options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseSqlite(connection)
                .Options;

            using CinemaDbContext context = new CinemaDbContext(options);
            context.Database.EnsureCreated();
        }

        public CinemaDbContext CreateContext() => new CinemaDbContext(options);

        public async Task Dispose() => await connection.DisposeAsync();
    }
}
