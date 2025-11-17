using System.Text;
using AttractionReviewAPI.Profiles;
using AttractionReviewAPI.Repositories;
using AttractionReviewAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AttractionReviewAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));
        
        var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>();
        var secretKey = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                        
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                        
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                        
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
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