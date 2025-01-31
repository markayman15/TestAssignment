using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using TestAssignment.Repository.Repositories;

namespace TestAssignment.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly CountryRepository BlockedCountriesRepo;
        private readonly TempBlockedReposatory tempBlockedReposatory;

        public CountriesController(CountryRepository blockedCountries,TempBlockedReposatory tempBlockedReposatory)
        {
            BlockedCountriesRepo = blockedCountries;
            this.tempBlockedReposatory = tempBlockedReposatory;
        }

        [HttpPost("block")]
        public ActionResult block([FromQuery]string Country_Code)
        {
            var item = new KeyValuePair<string,string>(Country_Code, "Blocked");
            var check = BlockedCountriesRepo.Add(item) ;
            if (check)
            {
                return Ok("Country Blocked Successfully");
            }
            return BadRequest();
        }

        [HttpDelete("block/{countryCode}")]
        public ActionResult DeleteCoumtry([FromRoute]string countryCode)
        {
            KeyValuePair<string, string> item = new KeyValuePair<string, string>(countryCode,"Blocked");
            
            bool check = BlockedCountriesRepo.CheckAvailability(item.Key);
            if (check)
            {
              
               var removed = BlockedCountriesRepo.Remove(item);
                if (removed)
                {
                    return Ok("Country blocking removed");
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpGet("blocked")]
        public ActionResult blocked([FromQuery]int? Page, int? PageSize,string CountryCode)
        {
            var result = BlockedCountriesRepo.GetWithSpec(Page,PageSize,CountryCode);
            return Ok(result);
        }
        [HttpPost("temporal-block")]
        public ActionResult BlockCountryTemp(string CountryCode, int durationMinutes)
        {
            if (durationMinutes < 1 || durationMinutes > 1440)
            {
                return BadRequest("Duration Must be from 1-1440");
            }

            if (!tempBlockedReposatory.countryCodes.Contains(CountryCode))
            {
                return BadRequest("Invalid Country Code");
            }

            var item = new KeyValuePair<string, int>(CountryCode, durationMinutes);
            tempBlockedReposatory.Add(item);
            BlockedCountriesRepo.Add(new KeyValuePair<string, string> (CountryCode,"Blocked"));
            return Ok($"Country Blocked Temporary For {durationMinutes} Minutes");
        }
    }
}
