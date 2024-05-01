using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RoadReady.DTO;
using RoadReady.Models;
using RoadReady.Repositories;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace RoadReady
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICarDetailRepository, CarDetailRepository>();
            builder.Services.AddScoped<ICarImageRepository, CarImageRepository>();
            builder.Services.AddScoped<ICarReviewRepository, CarReviewRepository>();
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IStateRepository, StateRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserTypeRepository, UserTypeRepository>();


          

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "jwtToken_Auth_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Here Enter JWT Token with bearer format like bearer[space] token"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference =  new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            builder.Services.AddDbContext<RoadReadyContext>();
           builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddCors(
                option =>
                {
                    option.AddDefaultPolicy(builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
                }
                );
            var app = builder.Build();

           /*/  void ConfigureServices(IServiceCollection services)
            {
                services.AddScoped<ICarRepository, CarRepository>();
                services.AddScoped<ICarDetailRepository, CarDetailRepository>();
                services.AddScoped<ICarImageRepository, CarImageRepository>();
                services.AddScoped<ICarReviewRepository, CarReviewRepository>();
                services.AddScoped<ICityRepository, CityRepository>();
                services.AddScoped<ILocationRepository, LocationRepository>();
                services.AddScoped<IPaymentDetailRepository, PaymentDetailRepository>();
                services.AddScoped<IReservationRepository, ReservationRepository>();
                services.AddScoped<IStateRepository, StateRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IUserTypeRepository, UserTypeRepository>();




            }*/
           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseAuthentication();

            app.UseAuthorization();
           

            app.MapControllers();

            app.Run();
        }
    }
}
