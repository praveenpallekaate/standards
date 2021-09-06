using System.Linq;
using System.Threading.Tasks;

namespace Core.Logger
{
    /// <summary>
    /// Interface for services working with <see cref="Log"/> entities.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Gets the <see cref="Log"/>.
        /// </summary>
        /// <returns>The <see cref="Log"/>.</returns>
        IQueryable<Log> Get();

        /// <summary>
        /// Deletes old log entries.
        /// </summary>
        /// <returns>Async task details.</returns>
        Task RotateLogs();

        /// <summary>
        /// Records a heartbeat to the logs.
        /// </summary>
        void Heartbeat(string app);
    }
}
