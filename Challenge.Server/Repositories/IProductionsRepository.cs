using Challenge.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Server.Repositories
{
    public interface IProductionsRepository
    {
        Task<Production> GetProduction(int identifier);
        Task<Production> CreateProduction(Production production);
        Task<Production> UpdateProduction(Production production);
        Task DeleteProduction(int identifier);

        // Listar
        Task<List<Tuple<string, string, DateTime>>> GetProductions();
        Task<List<Tuple<string, string, DateTime>>> GetProductions(string title, int genre, string order);
    }
}
