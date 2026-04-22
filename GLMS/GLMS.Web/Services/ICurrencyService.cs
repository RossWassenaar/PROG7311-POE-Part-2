/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

//Title: ExchangeRate-API Documentation
//Author: ExchangeRate-API
//Date: 20 April 2026
//Version: v6
//Availability: https://www.exchangerate-api.com/docs/overview


namespace GLMS.Web.Services
{
    public interface ICurrencyService
    {
        Task<decimal> GetUsdToZarRateAsync();
    }
}
