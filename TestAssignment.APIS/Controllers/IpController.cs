//using AutoMapper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TestAssignment.APIS.DTOs;
//using TestAssignment.APIS.Helpers;
using TestAssignment.Repository;
using TestAssignment.Repository.Repositories;

namespace TestAssignment.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        public CountryRepository CountryRepository { get; set; }
        //public IMapp mapper { get; set; }

        public IpController(CountryRepository countryRepository)
        {
            CountryRepository = countryRepository;
        }
        [HttpGet("Lookup")]
        public async Task<ActionResult> LookUp(string ipAddress, [FromServices] IHttpClientFactory httpClientFactory,[FromServices]IConfiguration configuration)
        {
            var key = configuration["APIKey"];
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={key}&ip={ipAddress}&fields=country_name,country_code3");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve IP data.");
            }

            var result = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<ResponseDTO>(result);

            return Ok(jsonResponse.country_name);
        }
        [HttpGet("check-block")]
        public async Task<ActionResult> CheckBlock([FromServices] IHttpClientFactory httpClientFactory,IConfiguration configuration)
        {
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var key = configuration["APIKey"];
            
            var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={key}&ip={userIp}&fields=country_name,country_code3,current_time");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve IP data.");
            }

            var result = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<ResponseDTO>(result);

            var check = CountryRepository.CheckAvailability(jsonResponse.country_code3);

            if (check)
            {
                var Log = new Log()
                {
                    Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                    country_code3 = jsonResponse.country_code3,
                    TimeStamp = jsonResponse.TimeStamp,
                    BlockStatus = "Blocked",
                    UserAgent = Request.Headers["User-Agent"].ToString()
                };
                return RedirectToAction("Add", "Logs", Log);
            }
            var newLog = new Log()
            {
                Ip = HttpContext.Connection.RemoteIpAddress.ToString(),
                country_code3 = jsonResponse.country_code3,
                TimeStamp = jsonResponse.TimeStamp,
                BlockStatus = "Not Blocked",
                UserAgent = Request.Headers["User-Agent"].ToString()
            };
            return RedirectToAction("Add", "Logs", newLog);

            //return Ok(jsonResponse);
        }
    }
}
