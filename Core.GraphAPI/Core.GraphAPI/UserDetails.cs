using Microsoft.Graph;

namespace Core.GraphAPI
{
    /// <summary>
    /// User detail model
    /// </summary>
    public class UserDetail
    {
        /// <summary>
        /// User photo as base64 string
        /// </summary>
        public string PhotoString { get; set; }

        /// <summary>
        /// User object
        /// </summary>
        public User User { get; set; }
    }
}
