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
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllWithRelationsAsync();
        Task<ServiceRequest?> GetByIdAsync(int id);
        Task<ServiceRequest?> GetDetailsAsync(int id);
        Task AddAsync(ServiceRequest request);
        void Remove(ServiceRequest request);
        Task SaveChangesAsync();
    }
}
