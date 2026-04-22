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

using System.Text.Json;

namespace GLMS.Web.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CurrencyService> _logger;

        //Fallback if the external API is unreachable
        private const decimal FallbackRate = 18.50m;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration, ILogger<CurrencyService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<decimal> GetUsdToZarRateAsync()
        {
            var apiKey = _configuration["ExchangeRateApi:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("ExchangeRateApi:ApiKey not configured. Using fallback rate.");
                return FallbackRate;
            }

            var url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/USD";

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                using var json = JsonDocument.Parse(response);

                return json.RootElement
                    .GetProperty("conversion_rates")
                    .GetProperty("ZAR")
                    .GetDecimal();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "ExchangeRate-API request failed. Falling back to {Rate}.", FallbackRate);
                return FallbackRate;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "ExchangeRate-API request timed out. Falling back to {Rate}.", FallbackRate);
                return FallbackRate;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "ExchangeRate-API returned unexpected JSON. Falling back to {Rate}.", FallbackRate);
                return FallbackRate;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "ZAR rate missing from ExchangeRate-API payload. Falling back to {Rate}.", FallbackRate);
                return FallbackRate;
            }
        }
    }
}
