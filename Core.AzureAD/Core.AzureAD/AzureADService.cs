using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.AzureAD
{
    /// <summary>
    /// Service to get Azure AD users
    /// </summary>
    public class AzureADService
    {
        private readonly GraphServiceClient _client;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="client">Graph client object</param>
        public AzureADService(GraphServiceClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Retrives current user with picture
        /// </summary>
        /// <returns>User details</returns>
        public async Task<UserDetail> GetCurrentUserWithPicAsync()
        {
            string photo = string.Empty;

            // Get current user
            var user = await _client.Me.Request().GetAsync();

            // Stream current user picture to string
            using (var photoStream = await _client.Me.Photo.Content.Request().GetAsync())
            {
                byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                photo = Convert.ToBase64String(photoByte);
            }

            return new UserDetail
            {
                PhotoString = photo,
                User = user
            };
        }

        /// <summary>
        /// Retrieves users details with picture
        /// </summary>
        /// <param name="email">Email as query</param>
        /// <returns>Users details</returns>
        public async Task<IEnumerable<UserDetail>> GetUsersWithPicForEmailAsync(string email)
        {
            List<UserDetail> result = new List<UserDetail>();

            // Filter users based on email
            var users = await _client.Users.Request().Filter($"startswith(mail, '{email}')").GetAsync();

            foreach (var user in users)
            {
                string photo = string.Empty;

                // Stream each user picture to string
                using (var photoStream = await _client.Users[user.Id].Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                    photo = Convert.ToBase64String(photoByte);
                }

                result.Add(new UserDetail
                {
                    PhotoString = photo,
                    User = user
                });
            }

            return result;
        }

        /// <summary>
        /// Retrieves users details
        /// </summary>
        /// <param name="email">Email as query</param>
        /// <returns>Users details</returns>
        public async Task<IEnumerable<UserDetail>> GetUsersForEmailAsync(string email)
        {

            var users = await _client.Users.Request().Filter($"startswith(mail, '{email}')").GetAsync();

            return users?
                .Select(i => new UserDetail
                {
                    PhotoString = string.Empty,
                    User = i
                });
        }
    }
}
