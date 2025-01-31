
using System.Collections.Concurrent;
//using TestAssignment.APIS.Helpers;
using TestAssignment.Repository.Repositories;
using TestAssignment.Services;

namespace TestAssignment.APIS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton(typeof(ConcurrentDictionary<,>));
            builder.Services.AddSingleton(typeof(GenericRepository<,>));
            builder.Services.AddSingleton(typeof(CountryRepository));
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton(typeof(List<>));
            builder.Services.AddSingleton(typeof(LogRepository));
            builder.Services.AddSingleton(typeof(TempBlockedReposatory));
            builder.Services.AddHostedService<AppBackgroundService>();
            //builder.Services.AddAutoMapper<AutoMapper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
