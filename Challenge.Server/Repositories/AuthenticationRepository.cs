using Challenge.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Challenge.Server.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly Database.MySql MySql;

        public AuthenticationRepository(Database.MySql mySql)
        {
            MySql = mySql;
        }

        public async Task<User> GetUser(int identifier)
        {
            User returnUser = null;

            Dictionary<string, object> parameters = new()
            {
                { "uID", identifier },
            };

            DataTable result = await MySql.ExecuteReader("GetUser", parameters);

            if (result.Rows.Count <= 0)
            {
                return returnUser;
            }

            DataRow row = result.Rows[0];
            returnUser = new()
            {
                ID = Convert.ToInt32(row["ID"]),
                Email = Convert.ToString(row["Email"])
            };

            return returnUser;
        }

        public async Task<User> Login(Authentication credentials)
        {
            User returnUser = null;

            if (!credentials.Validate())
            {
                return returnUser;
            }

            Dictionary<string, object> parameters = new()
            {
                { "uEmail", credentials.Email },
                { "uPassword", credentials.Password }
            };

            DataTable result = await MySql.ExecuteReader("Login", parameters);

            if (result.Rows.Count == 0)
            {
                return returnUser;
            }

            return await GetUser(Convert.ToInt32(result.Rows[0]["ID"]));
        }

        public async Task<User> Register(Authentication credentials)
        {
            User returnUser = null;

            if (!credentials.Validate())
            {
                return returnUser;
            }

            Dictionary<string, object> parameters = new()
            {
                { "uEmail", credentials.Email },
                { "uPassword", credentials.Password }
            };

            int identifier = Convert.ToInt32(await MySql.ExecuteScalar("Register", parameters));

            if (identifier <= 0)
            {
                return returnUser;
            }

            return await GetUser(identifier);
        }
    }
}
