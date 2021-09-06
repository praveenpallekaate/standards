using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Logger
{
    /// <summary>
    /// Represnts a log event within the system.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="timeStamp">The time the entry occurred at.</param>
        public Log(DateTimeOffset timeStamp)
        {
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// Gets the unique store identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the message of the log entry.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the template of the message.
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// Gets or sets the level of the log entry.
        /// </summary>
        [MaxLength(128)]
        public string Level { get; set; }

        /// <summary>
        /// Gets the time the entry occurred at.
        /// </summary>
        public DateTimeOffset TimeStamp { get; private set; }

        /// <summary>
        /// Gets or sets the text of the exception associated with the log.
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the XML string of any properties associated with the log.
        /// </summary>
        public string Properties { get; set; }
    }
}
