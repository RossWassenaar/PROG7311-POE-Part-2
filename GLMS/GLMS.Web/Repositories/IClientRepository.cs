/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Models;

namespace GLMS.Web.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> GetAllWithContractsAsync();
        Task<Client?> GetByIdAsync(int id);
        Task<Client?> GetByIdWithContractsAsync(int id);
        Task AddAsync(Client client);
        void Update(Client client);
        void Remove(Client client);
        Task SaveChangesAsync();
    }
}
