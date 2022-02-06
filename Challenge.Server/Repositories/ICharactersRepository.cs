using Challenge.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Server.Repositories
{
    public interface ICharactersRepository
    {
        Task<Character> GetCharacter(int identifier);
        Task<Character> CreateCharacter(Character character);
        Task<Character> UpdateCharacter(Character character);
        Task DeleteCharacter(int identifier);

        // Listar
        Task<Dictionary<string, string>> GetCharacters();
        Task<Dictionary<string, string>> GetCharacters(string name, int age, int movies);
    }
}
