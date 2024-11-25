﻿using ECommerce.API.Errors;
using ECommerce.API.Helpers;
using ECommerce.Business.Services;
using ECommerce.Core;
using ECommerce.Core.AutoMapper;
using ECommerce.Core.Entities.Identity;
using ECommerce.Core.Repositories;
using ECommerce.Core.Services;
using ECommerce.DataAccess;
using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Identity;
using ECommerce.DataAccess.Reposioties;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace ECommerce.API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services,IConfiguration configurations)
        {
            // AppDbContext
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configurations.GetConnectionString("Default"));
            });

            // AppIdentityDbContext
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(configurations.GetConnectionString("Identity"));
            });

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                return ConnectionMultiplexer.Connect(configurations.GetConnectionString("Redis"));
            });

            // Repositories
            services.AddScoped<IBasketRepository,BasketRepository>();
            
            // UnitOfWorks
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            // Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddSingleton<IResponseCacheService,ResponseCacheService>();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Handle Validation Error
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors).Select(e => e.ErrorMessage).ToList();

                    var vaildationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(vaildationErrorResponse);
                };

            });

            // Configure services of identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            // Jwt
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options=>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configurations["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configurations["JWT:Audience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations["JWT:Key"]))
                    };
                }
                );

            

            return services;
        }
    }
}
