/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */
//Title: ASP.NET Core Fundamentals Overview
//Author: Microsoft
//Date: 20 April 2026
//Version: .NET 10
//Availability: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-10.0&tabs=windows

using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;

        public ClientRepository(AppDbContext context) => _context = context;

        public Task<List<Client>> GetAllWithContractsAsync() =>
            _context.Clients.Include(c => c.Contracts).OrderBy(c => c.Name).ToListAsync();

        public Task<Client?> GetByIdAsync(int id) =>
            _context.Clients.FirstOrDefaultAsync(c => c.Id == id);

        public Task<Client?> GetByIdWithContractsAsync(int id) =>
            _context.Clients.Include(c => c.Contracts).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Client client) => await _context.Clients.AddAsync(client);

        public void Update(Client client) => _context.Clients.Update(client);

        public void Remove(Client client) => _context.Clients.Remove(client);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
