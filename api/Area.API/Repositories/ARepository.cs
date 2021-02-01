using System;
using Area.API.DbContexts;

namespace Area.API.Repositories
{
    public class ARepository : IDisposable
    {
        protected readonly AreaDbContext Database;

        protected ARepository(AreaDbContext database)
        {
            Database = database;
        }

        public void Dispose()
        {
            try {
                Database.SaveChanges();
            } catch (ObjectDisposedException) {
                // ignored
            }
        }
    }
}