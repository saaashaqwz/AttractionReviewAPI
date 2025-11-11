using AttractionReviewAPI.Profiles;
using AttractionReviewAPI.Repositories;
using AttractionReviewAPI.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AttractionReviewAPI;

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
        
        builder.Services.AddDbContext<APIDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
        
        builder.Services.AddScoped<IAttractionRepository, AttractionRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        
        ILoggerFactory factory = new LoggerFactory();
        builder.Services.AddSingleton<IMapper>(_ =>
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ReviewProfile>();
            }, factory);
            return configuration.CreateMapper();
        });
        
        builder.Services.AddScoped<IAttractionService, AttractionService>();
        builder.Services.AddScoped<IReviewService, ReviewService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();

        app.Run();
    }
}