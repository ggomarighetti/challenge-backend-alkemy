using Challenge.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Challenge.Server.Repositories
{
    public class ProductionsRepository : IProductionsRepository
    {
        private readonly Database.MySql MySql;

        public ProductionsRepository(Database.MySql mySql)
        {
            MySql = mySql;
        }

        public async Task<Production> CreateProduction(Production production)
        {
            Production returnProduction = null;

            if (!production.Validate())
            {
                return returnProduction;
            }

            Dictionary<string, object> parameters = new()
            {
                { "pImage", production.Image },
                { "pTitle", production.Title },
                { "pLaunch", production.Launch },
                { "pRating", production.Rating },
            };

            int identifier = Convert.ToInt32(await MySql.ExecuteScalar("CreateProduction", parameters));

            if (identifier <= 0)
            {
                return returnProduction;
            }

            return await GetProduction(identifier);
        }

        public async Task DeleteProduction(int identifier)
        {
            await MySql.ExecuteQuery("DeleteProduction", new() { { "pID", identifier } });
        }

        public async Task<Production> GetProduction(int identifier)
        {
            Production returnProduction = null;

            Dictionary<string, object> parameters = new()
            {
                { "pID", identifier }
            };

            DataTable result = await MySql.ExecuteReader("GetProduction", parameters);

            if (result.Rows.Count < 0)
            {
                return returnProduction;
            }

            DataRow row = result.Rows[0];
            returnProduction = new()
            {
                Image = Convert.ToString(row["Image"]),
                Title = Convert.ToString(row["Name"]),
                Launch = Convert.ToDateTime(row["Launch"]),
                Rating = Convert.ToDecimal(row["Rating"]),
            };

            return returnProduction;
        }

        public async Task<List<Tuple<string, string, DateTime>>> GetProductions()
        {
            List<Tuple<string, string, DateTime>> returnProductions = new();

            string query = "SELECT ID FROM productions";

            DataTable result = await MySql.ExecuteReader(query, type: CommandType.Text);

            foreach (DataRow row in result.Rows)
            {
                Production auxProduction = await GetProduction(Convert.ToInt32(row["ID"]));
                returnProductions.Add(new(auxProduction.Image, auxProduction.Title, auxProduction.Launch));
            }

            return returnProductions;
        }

        public async Task<List<Tuple<string, string, DateTime>>> GetProductions(string title, int genre, string order)
        {
            List<Tuple<string, string, DateTime>> returnProductions = new();

            if (string.IsNullOrEmpty(title) && genre <= 0 && string.IsNullOrEmpty(order))
            {
                return returnProductions;
            }

            string query = "SELECT ID FROM productions WHERE ";

            if (!string.IsNullOrEmpty(title))
            {
                query += "Title LIKE pTitle ";
            }

            if (genre > 0)
            {
                //TODO
            }

            if (!string.IsNullOrEmpty(order))
            {
                query += $"ORDER BY {order} ";
            }

            Dictionary<string, object> parameters = new()
            {
                { "pTitle", $"%{title}%" },
            };

            DataTable result = await MySql.ExecuteReader(query, type: CommandType.Text);

            foreach (DataRow row in result.Rows)
            {
                Production auxProduction = await GetProduction(Convert.ToInt32(row["ID"]));
                returnProductions.Add(new(auxProduction.Image, auxProduction.Title, auxProduction.Launch));
            }

            return returnProductions;
        }

        public async Task<Production> UpdateProduction(Production production)
        {
            Production returnProduction = null;

            if (!production.Validate())
            {
                return returnProduction;
            }

            Dictionary<string, object> parameters = new()
            {
                { "pID", production.ID },
                { "pImage", production.Image },
                { "pTitle", production.Title },
                { "pLaunch", production.Launch },
                { "pRating", production.Rating },
            };

            await MySql.ExecuteQuery("UpdateProduction", parameters);
            return await GetProduction(production.ID);
        }
    }
}
