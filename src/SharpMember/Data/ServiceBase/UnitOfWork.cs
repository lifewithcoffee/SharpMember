using Microsoft.EntityFrameworkCore;
using SharpCommons;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpMember.Data.ServiceBase
{
    public interface IUnitOfWork<TDbContext> : IDisposable where TDbContext : DbContext
    {
        bool Commit();
        Task<bool> CommitAsync();
        TDbContext Context { get; }
    }

    public class UnitOfWork : IUnitOfWork<ApplicationDbContext>
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Logger.WriteTrace("Initializing UnitOfWork with system default setting");
        }

        /// <summary>
        /// This method is for unit test.
        /// </summary>
        public UnitOfWork(string connectionString)
        {
            Ensure.IsTrue(!string.IsNullOrWhiteSpace(connectionString));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(new SqlConnection(connectionString))
                .Options;

            _context = new ApplicationDbContext(options);

            Logger.WriteTrace("Initializing UnitOfWork with connection string: {0}", connectionString);
        }

        public ApplicationDbContext Context { get { return _context; } }

        public async Task<bool> CommitAsync()
        {
            bool result = false;
            try
            {
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex);
            }

            return result;
        }

        public bool Commit()
        {
            bool result = false;
            try
            {
                _context.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                Logger.WriteException(ex);
            }

            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
