using Core.LDAP;
using System;

namespace TestLDAP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started LDAP Test!");

            Console.WriteLine("Enter LDAP URL: ");
            string ldapURL = Console.ReadLine();

            Console.WriteLine("Enter LDAP username: ");
            string ldapUsername = Console.ReadLine();

            Console.WriteLine("Enter LDAP pass: ");
            string ldapPass = Console.ReadLine();

            if (!string.IsNullOrEmpty(ldapURL) && !string.IsNullOrEmpty(ldapUsername) && !string.IsNullOrEmpty(ldapPass))
            {
                LDAPService service = new LDAPService(ldapURL, ldapUsername, ldapPass);

                if (service is LDAPService)
                {
                    Console.WriteLine("Enter AD 4*4 / username");
                    string username = Console.ReadLine();

                    try
                    {
                        var user = service.GetADUserDetailsFor(username);

                        if (user is ADUser)
                        {
                            Console.WriteLine($"Found user details:: Name: {user.SirName}, {user.SirName} Email: {user.Email}!!");
                        }
                        else
                        {
                            Console.WriteLine($"Unable to find user {username}!!");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error {ex.Message}!!");
                        Console.WriteLine($"Unable to find user {username}!!"); ;
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("Config param provided was empty!!");
            }

            Console.ReadLine();
        }
    }
}
