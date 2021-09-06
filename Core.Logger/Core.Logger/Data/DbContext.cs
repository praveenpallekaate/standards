using Microsoft.EntityFrameworkCore;

namespace Core.Logger
{
    /// <summary>
    /// Database context for EmployeeIntegrityTool.
    /// </summary>
    public class LogsDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContext"/> class.
        /// </summary>
        /// <param name="contextOptions">Database context options.</param>
        public LogsDbContext(DbContextOptions<LogsDbContext> contextOptions)
            : base(contextOptions)
        {
        }

        /// <summary>
        /// Gets or sets <see cref="Log"/> entities.
        /// </summary>
        public virtual DbSet<Log> Logs { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
