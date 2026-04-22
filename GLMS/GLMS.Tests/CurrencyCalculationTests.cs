/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Services;
using Xunit;

namespace GLMS.Tests
{
    public class CurrencyCalculationTests
    {
        [Fact]
        public void ConvertUsdToZar_GivenRate18_5_Returns1850ForUsd100()
        {
            decimal usdAmount = 100m;
            decimal exchangeRate = 18.5m;


            decimal result = CurrencyCalculator.ConvertUsdToZar(usdAmount, exchangeRate);
            Assert.Equal(1850.00m, result);
        }

        [Fact]
        public void ConvertUsdToZar_ZeroAmount_ReturnsZero()
        {
            decimal result = CurrencyCalculator.ConvertUsdToZar(0m, 18.5m);
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ConvertUsdToZar_ZeroRate_ReturnsZero()
        {
            decimal result = CurrencyCalculator.ConvertUsdToZar(500m, 0m);
            Assert.Equal(0m, result);
        }

        [Fact]
        public void ConvertUsdToZar_RoundsToTwoDecimalPlaces()
        {
            decimal result = CurrencyCalculator.ConvertUsdToZar(1m, 18.333333m);
            Assert.Equal(18.33m, result);
        }

        [Fact]
        public void ConvertUsdToZar_NegativeAmount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                CurrencyCalculator.ConvertUsdToZar(-50m, 18.5m));
        }

        [Fact]
        public void ConvertUsdToZar_NegativeRate_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                CurrencyCalculator.ConvertUsdToZar(100m, -1m));
        }

        [Fact]
        public void ConvertUsdToZar_LargeAmount_CalculatesCorrectly()
        {
            decimal result = CurrencyCalculator.ConvertUsdToZar(50000m, 18.5m);
            Assert.Equal(925000.00m, result);
        }
    }
}
