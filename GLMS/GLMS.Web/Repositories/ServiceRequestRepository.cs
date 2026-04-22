/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Repositories
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AppDbContext _context;

        public ServiceRequestRepository(AppDbContext context) => _context = context;

        public Task<List<ServiceRequest>> GetAllWithRelationsAsync() =>
            _context.ServiceRequests
                .Include(s => s.Contract)
                    .ThenInclude(c => c!.Client)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

        public Task<ServiceRequest?> GetByIdAsync(int id) =>
            _context.ServiceRequests.FirstOrDefaultAsync(s => s.Id == id);

        public Task<ServiceRequest?> GetDetailsAsync(int id) =>
            _context.ServiceRequests
                .Include(s => s.Contract)
                    .ThenInclude(c => c!.Client)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(ServiceRequest request) => await _context.ServiceRequests.AddAsync(request);

        public void Remove(ServiceRequest request) => _context.ServiceRequests.Remove(request);

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
