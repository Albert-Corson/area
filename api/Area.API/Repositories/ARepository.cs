using System;
using System.Linq;
using Area.API.DbContexts;
using Microsoft.EntityFrameworkCore;

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
            bool saveFailed;
            do {
                saveFailed = false;

                try {
                    Database.SaveChanges();
                } catch (DbUpdateConcurrencyException ex) {
                    saveFailed = true;
                    ex.Entries.Single().Reload();
                } catch (ObjectDisposedException) {
                    
                }
            } while (saveFailed);
        }
    }
}