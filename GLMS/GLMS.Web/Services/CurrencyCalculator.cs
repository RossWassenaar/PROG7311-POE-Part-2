/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

namespace GLMS.Web.Services
{
    public static class CurrencyCalculator
    {
        public static decimal ConvertUsdToZar(decimal usdAmount, decimal exchangeRate)
        {
            if (usdAmount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(usdAmount));
            if (exchangeRate < 0)
                throw new ArgumentException("Exchange rate cannot be negative.", nameof(exchangeRate));

            return Math.Round(usdAmount * exchangeRate, 2);
        }
    }
}
