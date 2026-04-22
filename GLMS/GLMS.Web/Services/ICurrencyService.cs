/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

namespace GLMS.Web.Services
{
    public interface ICurrencyService
    {
        Task<decimal> GetUsdToZarRateAsync();
    }
}
