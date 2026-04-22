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
    public class ContractRepository : IContractRepository
    {
        private readonly AppDbContext _context;

        public ContractRepository(AppDbContext context) => _context = context;

        //LINQ-based search by Date Range and Status.
        public async Task<List<Contract>> SearchAsync(string? status, DateTime? startFrom, DateTime? startTo)
        {
            var query = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (startFrom.HasValue)
                query = query.Where(c => c.StartDate >= startFrom.Value);

            if (startTo.HasValue)
                query = query.Where(c => c.StartDate <= startTo.Value);

            return await query.OrderByDescending(c => c.StartDate).ToListAsync();
        }

        public Task<Contract?> GetByIdAsync(int id) =>
            _context.Contracts.FirstOrDefaultAsync(c => c.Id == id);

        public Task<Contract?> GetDetailsAsync(int id) =>
            _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);

        public Task<List<Contract>> GetActiveForDropdownAsync() =>
            _context.Contracts
                .Include(c => c.Client)
                .Where(c => c.Status != "Expired" && c.Status != "On Hold")
                .OrderBy(c => c.Client!.Name)
                .ToListAsync();

        public async Task AddAsync(Contract contract) => await _context.Contracts.AddAsync(contract);

        public void Remove(Contract contract) => _context.Contracts.Remove(contract);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
