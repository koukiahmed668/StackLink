using UserService.Models;
using UserService.Repositories;
using UserService.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System;
using UserService.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            DotNetEnv.Env.Load();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add DbContext and repository
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(connectionString));

            // JWT Configuration
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            // Override SecretKey from environment variable
            var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (string.IsNullOrWhiteSpace(jwtSecretKey))
            {
                throw new Exception("JWT_SECRET_KEY is missing or empty in environment variables.");
            }
            jwtSettings.SecretKey = jwtSecretKey;

            builder.Services.Configure<JwtSettings>(options =>
            {
                options.Issuer = jwtSettings.Issuer;
                options.Audience = jwtSettings.Audience;
                options.ExpiryMinutes = jwtSettings.ExpiryMinutes;
                options.SecretKey = jwtSecretKey;
            });
            builder.Services.AddSingleton<TokenService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            // Add services to the container.
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ProfileService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
