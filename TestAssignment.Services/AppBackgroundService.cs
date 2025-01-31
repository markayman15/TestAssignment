using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssignment.Repository.Repositories;

namespace TestAssignment.Services
{
    public class AppBackgroundService : BackgroundService
    {
        public CountryRepository CountryRepository { get; set; }
        public TempBlockedReposatory TempBlockedReposatory { get; set; }
        public AppBackgroundService(CountryRepository countryRepository, TempBlockedReposatory tempBlockedReposatory)
        {
            CountryRepository = countryRepository;
            TempBlockedReposatory = tempBlockedReposatory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5 * 60000);
                TempBlockedReposatory.DecreaseBlockTime();
                var RemovedKeys = TempBlockedReposatory.RemoveExpiredBlock();
                foreach(var key in RemovedKeys)
                {
                    CountryRepository.Remove(new KeyValuePair<string, string>(key, "Blocked"));
                }
            }
        }
    }
}
