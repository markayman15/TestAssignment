using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignment.Repository.Repositories
{
    public class LogRepository
    {
        private readonly List<Log> logs;

        public LogRepository(List<Log> logs)
        {
            this.logs = logs;
        }

        public List<Log> GetLogs(int? Page, int? PageSize)
        {
            if(Page.HasValue && PageSize.HasValue)
            {
                return logs.Skip((Page.Value - 1) * PageSize.Value).Take(PageSize.Value).Where(l=>l.BlockStatus == "Blocked").ToList();
            }
            return logs.Where(l=>l.BlockStatus == "Blocked").ToList();
        }

        public void Add(Log log)
        {
            logs.Add(log);
        }
    }
}
