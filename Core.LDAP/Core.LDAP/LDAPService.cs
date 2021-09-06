using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace Core.LDAP
{
    /// <summary>
    /// LDAP service
    /// </summary>
    public class LDAPService
    {
        DirectoryEntry directoryEntry = null;

        readonly DirectorySearcher _directorySearcher = null;
        readonly string _ldapPath = string.Empty;

        const string givenName = "givenName";
        const string mail = "mail";
        const string sirName = "sn";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ldapPath">LDAP path</param>
        public LDAPService(string ldapPath)
        {
            _ldapPath = ldapPath;

            directoryEntry = new DirectoryEntry(_ldapPath);
            _directorySearcher = new DirectorySearcher(directoryEntry);
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ldapPath"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public LDAPService(string ldapPath, string userName, string password)
        {
            _ldapPath = ldapPath;

            directoryEntry = new DirectoryEntry(_ldapPath, userName, password);
            _directorySearcher = new DirectorySearcher(directoryEntry);
        }

        /// <summary>
        /// Fetch user details from LDAP
        /// </summary>
        /// <param name="name">User name</param>
        /// <returns>LDAP user details</returns>
        public ADUser GetADUserDetailsFor(string name)
        {
            ADUser result = null;

            if (!string.IsNullOrEmpty(name))
            {
                // Set filter
                _directorySearcher.Filter = GetFilter(name);

                // Search user
                SearchResult searchResult = _directorySearcher.FindOne();

                directoryEntry = searchResult?.GetDirectoryEntry();

                result = new ADUser
                {
                    Email = directoryEntry?.Properties[mail]?.Value?.ToString(),
                    GivenName = directoryEntry?.Properties[givenName]?.Value?.ToString(),
                    SirName = directoryEntry?.Properties[sirName]?.Value?.ToString()
                };
            }

            return result;
        }

        /// <summary>
        /// Search for user
        /// </summary>
        /// <param name="query">Query for user</param>
        /// <returns>LDAP user details</returns>
        public IEnumerable<ADUser> SearchADUserDetailsFor(string query)
        {
            List<ADUser> result = new List<ADUser>();

            if (!string.IsNullOrEmpty(query))
            {
                var usersGivenName = SearchADUserDetailsForFilter(GetSearchGivenNameFilter(query));

                if (usersGivenName != null && usersGivenName.Any())
                {
                    result = usersGivenName.ToList();
                }
                else
                {
                    var usersSirName = SearchADUserDetailsForFilter(GetSearchSirNameFilter(query));

                    if (usersSirName != null && usersSirName.Any())
                    {
                        result = usersSirName.ToList();
                    }
                    else
                    {
                        var usersMail = SearchADUserDetailsForFilter(GetSearchMailFilter(query));

                        if (usersMail != null && usersMail.Any())
                        {
                            result = usersMail.ToList();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Fetch user details from LDAP
        /// </summary>
        /// <param name="names">User names</param>
        /// <returns>LDAP user details collection</returns>
        public IEnumerable<ADUser> GetADUsersDetailsForNames(string[] names)
        {
            IEnumerable<ADUser> result = null;

            if (names != null && names.Any())
            {
                List<ADUser> users = new List<ADUser>();

                foreach (var name in names)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        // Set filter
                        _directorySearcher.Filter = GetFilter(name);

                        // Search user
                        SearchResult searchResult = _directorySearcher.FindOne();

                        directoryEntry = searchResult?.GetDirectoryEntry();

                        if (directoryEntry != null)
                        {
                            users.Add(new ADUser
                            {
                                Email = directoryEntry.Properties[mail]?.Value?.ToString(),
                                GivenName = directoryEntry.Properties[givenName]?.Value?.ToString(),
                                SirName = directoryEntry.Properties[sirName]?.Value?.ToString()
                            });
                        }
                    }
                }

                result = users;
            }

            return result;
        }

        /// <summary>
        /// Search users for filter
        /// </summary>
        /// <param name="filter">Filter with query</param>
        /// <returns>User list</returns>
        private IEnumerable<ADUser> SearchADUserDetailsForFilter(string filter)
        {
            List<ADUser> result = new List<ADUser>();

            if (!string.IsNullOrEmpty(filter))
            {
                // Set filter
                _directorySearcher.Filter = filter;

                // Search users
                SearchResultCollection searchResultCollection = _directorySearcher.FindAll();

                foreach (SearchResult searchResult in searchResultCollection)
                {
                    directoryEntry = searchResult.GetDirectoryEntry();

                    result.Add(new ADUser
                    {
                        Email = directoryEntry?.Properties[mail]?.Value?.ToString(),
                        GivenName = directoryEntry?.Properties[givenName]?.Value?.ToString(),
                        SirName = directoryEntry?.Properties[sirName]?.Value?.ToString()
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Filter for 4*4
        /// </summary>
        /// <param name="name"><User 4*4/param>
        /// <returns>Filter string</returns>
        private string GetFilter(string name)
        {
            return $"(sAMAccountName={name})";
        }

        /// <summary>
        /// Filter for givenname
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Filter string</returns>
        private string GetSearchGivenNameFilter(string query)
        {
            return string.Format("(&(objectCategory=person)(objectClass=user)(givenname={0}))", query);
        }

        /// <summary>
        /// Filter for sirname
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Filter string</returns>
        private string GetSearchSirNameFilter(string query)
        {
            return string.Format("(&(objectCategory=person)(objectClass=user)(sn={0}))", query);
        }

        /// <summary>
        /// Filter for mail
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Filter string</returns>
        private string GetSearchMailFilter(string query)
        {
            return string.Format("(&(objectCategory=person)(objectClass=user)(mail={0}))", query);
        }
    }
}
