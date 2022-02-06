using Challenge.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Challenge.Server.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly Database.MySql MySql;

        public CharacterRepository(Database.MySql mySql)
        {
            MySql = mySql;
        }

        public async Task<Character> CreateCharacter(Character character)
        {
            Character returnCharacter = null;

            if (!character.Validate())
            {
                return returnCharacter;
            }

            Dictionary<string, object> parameters = new()
            {
                { "cImage", character.Image },
                { "cName", character.Name },
                { "cAge", character.Age },
                { "cWeight", character.Weight },
                { "cHistory", character.History },
            };

            int identifier = Convert.ToInt32(await MySql.ExecuteScalar("CreateCharacter", parameters));

            if (identifier <= 0)
            {
                return returnCharacter;
            }

            return await GetCharacter(identifier);
        }

        public async Task DeleteCharacter(int identifier)
        {
            await MySql.ExecuteQuery("DeleteCharacter", new() { { "cID", identifier } });
        }

        public async Task<Character> GetCharacter(int identifier)
        {
            Character returnCharacter = null;

            Dictionary<string, object> parameters = new()
            {
                { "cID", identifier }
            };

            DataTable result = await MySql.ExecuteReader("GetCharacter", parameters);

            if (result.Rows.Count < 0)
            {
                return returnCharacter;
            }

            DataRow row = result.Rows[0];
            returnCharacter = new()
            {
                Image = Convert.ToString(row["Image"]),
                Name = Convert.ToString(row["Name"]),
                Age = Convert.ToInt32(row["Age"]),
                Weight = Convert.ToDecimal(row["Weight"]),
                History = Convert.ToString(row["History"])
            };

            return returnCharacter;
        }

        public async Task<Dictionary<string, string>> GetCharacters()
        {
            Dictionary<string, string> returnCharacters = new();

            string query = "SELECT ID FROM characters";

            DataTable result = await MySql.ExecuteReader(query, type: CommandType.Text);

            foreach (DataRow row in result.Rows)
            {
                Character auxCharacter = await GetCharacter(Convert.ToInt32(row["ID"]));
                returnCharacters.Add(auxCharacter.Image, auxCharacter.Name);
            }

            return returnCharacters;
        }

        public async Task<Dictionary<string, string>> GetCharacters(string name, int age, int movies)
        {
            Dictionary<string, string> returnCharacters = new();

            if (string.IsNullOrEmpty(name) && age <= 0 && movies <= 0)
            {
                return returnCharacters;
            }

            string query = "SELECT ID FROM characters WHERE ";

            if (!string.IsNullOrEmpty(name))
            {
                query += "Name LIKE cName ";
            }

            if (age > 0)
            {
                query += "Age = cAge ";
            }

            if (movies > 0)
            {
                //TODO
            }

            Dictionary<string, object> parameters = new()
            {
                { "cName", $"%{name}%" },
                { "cAge", age }
            };

            DataTable result = await MySql.ExecuteReader(query, type: CommandType.Text);

            foreach (DataRow row in result.Rows)
            {
                Character auxCharacter = await GetCharacter(Convert.ToInt32(row["ID"]));
                returnCharacters.Add(auxCharacter.Image, auxCharacter.Name);
            }

            return returnCharacters;

        }

        public async Task<Character> UpdateCharacter(Character character)
        {
            Character returnCharacter = null;

            if (!character.Validate())
            {
                return returnCharacter;
            }

            Dictionary<string, object> parameters = new()
            {
                { "cID", character.ID },
                { "cImage", character.Image },
                { "cName", character.Name },
                { "cAge", character.Age },
                { "cWeight", character.Weight },
                { "cHistory", character.History },
            };

            await MySql.ExecuteQuery("UpdateCharacter", parameters);
            return await GetCharacter(character.ID);
        }
    }
}
