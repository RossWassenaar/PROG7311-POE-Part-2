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

using GLMS.Web.Models;

namespace GLMS.Web.Repositories
{
    public interface IContractRepository
    {
        Task<List<Contract>> SearchAsync(string? status, DateTime? startFrom, DateTime? startTo);
        Task<Contract?> GetByIdAsync(int id);
        Task<Contract?> GetDetailsAsync(int id);
        Task<List<Contract>> GetActiveForDropdownAsync();
        Task AddAsync(Contract contract);
        void Remove(Contract contract);
        Task SaveChangesAsync();
    }
}
