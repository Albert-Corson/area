using System;
using Area.API.Database;

namespace Area.API.Repositories
{
    public class ARepository : IDisposable
    {
        protected readonly DbContext Database;

        protected ARepository(DbContext database)
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