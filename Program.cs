using JwtCrud.middleware;
using JwtCrud.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace JwtCrud
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
        });              
               
            builder.Services.AddControllers();           
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<CustomExceptionFilter>();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(c => {
                c.AddPolicy("AllowOrigin", options => options.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyMethod()
                .AllowAnyHeader().AllowCredentials());
            });
            builder.Services.AddTransient<GlobalErrorHandler>();
            builder.Services.AddDbContext<JwtTokenContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("mvcConnection")));
            var app = builder.Build();           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseExceptionMiddlewear();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowOrigin"); 
            app.MapControllers();
            app.Run();

        }
    }
}