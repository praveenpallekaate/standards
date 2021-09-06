using System;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Logger
{
    /// <summary>
    /// Service to interact with <see cref="Log"/> entities.
    /// </summary>
    public class LogService : ILogService
    {
        /// <summary>
        /// The pipeline to the <see cref="LogsDbContext"/>.
        /// </summary>
        private readonly LogsDbContext _context;

        /// <summary>
        /// The pipeline to the <see cref="ILogger{LogService}"/>
        /// </summary>
        private readonly ILogger<LogService> _logger;

        /// <summary>
        /// Number of days to keep logs for.
        /// </summary>
        private readonly int _daysToKeepLogs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogService"/> class.
        /// </summary>
        /// <param name="context">The injected <see cref="LogsDbContext"/>.</param>
        /// <param name="logger">The injected <see cref="ILogger{LogService}"/>.</param>
        /// <param name="logOptions">Log options.</param>
        public LogService(LogsDbContext context, ILogger<LogService> logger, IOptions<LogOptions> logOptions)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _daysToKeepLogs = logOptions.Value.DaysToKeep;
        }

        /// <summary>
        /// Gets the <see cref="Log"/>.
        /// </summary>
        /// <returns>The <see cref="Log"/>.</returns>
        public IQueryable<Log> Get()
        {
            return _context.Logs.AsNoTracking();
        }

        /// <summary>
        /// Deletes old log entries.
        /// </summary>
        /// <returns>Async task details.</returns>
        public async Task RotateLogs()
        {
            var deleteLogsBefore = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(_daysToKeepLogs));

            var logsToDelete = _context.Logs
                .AsNoTracking()
                .Where(log => log.TimeStamp < deleteLogsBefore)
                .ToList();

            _context.BulkDelete(logsToDelete);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Records a heartbeat to the logs.
        /// </summary>
        public void Heartbeat(string app)
        {
            _logger.LogInformation($"{app} Heartbeat");
        }

        /// <summary>
        /// Main is for CI build
        /// </summary>
        private static void Main() { }
    }
}
