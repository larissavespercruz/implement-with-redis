using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http;
using Newtonsoft.Json;
using implement_with_redis.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace implement_with_redis.Controllers
{
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private const string CountriesKey = "Countries";

        public CountriesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countriesObject = await _distributedCache.GetStringAsync(CountriesKey);

            if (!string.IsNullOrWhiteSpace(countriesObject))
            {
                return Ok(countriesObject);
            }
            else
            {
                const string restCountriesUrl = "https://restcountries.com/v3.1/all";
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(restCountriesUrl);
                    var responseData = await response.Content.ReadAsStringAsync();
                    var countries = JsonConvert.DeserializeObject<List<Country>>(responseData);
                    var memoryCacheEntryOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                        SlidingExpiration = TimeSpan.FromSeconds(1200)
                    };

                    await _distributedCache.SetStringAsync(CountriesKey, responseData, memoryCacheEntryOptions);

                    return Ok(countries);
                }
            }

        }
    }
}
