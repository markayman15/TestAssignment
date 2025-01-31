using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAssignment.Repository;
using TestAssignment.Repository.Repositories;

namespace TestAssignment.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        public LogRepository LogRepository { get; set; }
        public LogsController(LogRepository logRepository) 
        {
            this.LogRepository = logRepository;
        }
        [HttpGet]
        public ActionResult GetLogs(int? Page, int? PageSize)
        {
            var result = LogRepository.GetLogs(Page, PageSize);
            return Ok(result);
        }
        [HttpPost]
        public ActionResult AddLog(Log log)
        {
            LogRepository.Add(log);
            return Ok("Log Add Successfully");
        }
    }
}
