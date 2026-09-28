using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nokubico.Application.Interfaces;
using Nokubico.Domain.Account;
using Nokubico.Domain.Interface.Auth;
using Nokubico.Domain.Interface.Companies;
using Nokubico.Domain.Interface.Community;
using Nokubico.Domain.Interface.Marketplace;
using Nokubico.Domain.Interface.Messaging;
using Nokubico.Domain.Interface.Wallets;
using Nokubico.Infra.Data.Context;
using Nokubico.Infra.Data.Identity;
using Nokubico.Infra.Data.Repository.Auth;
using Nokubico.Infra.Data.Repository.Companies;
using Nokubico.Infra.Data.Repository.Community;
using Nokubico.Infra.Data.Repository.Marketplace;
using Nokubico.Infra.Data.Repository.Messaging;
using Nokubico.Infra.Data.Repository.Wallets;
using Nokubico.Infra.Services.Storage;

namespace Nokubico.Infra.Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connection))
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connection, b => b.MigrationsAssembly("Nokubico.Infra.Data"))
                );
            }

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? string.Empty)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IUserRepository>(r => new UserRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<ISessionRepository>(r => new SessionRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<IAccountRepository>(r => new AccountRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<IPostRepository>(r => new PostRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<IProductRepository>(r => new ProductRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<IOrderRepository>(r => new OrderRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<IWalletRepository>(r => new WalletRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<IConversationRepository>(r => new ConversationRepository(r.GetRequiredService<AppDbContext>()));
            services.AddScoped<ICompanyRepository>(r => new CompanyRepository(r.GetRequiredService<AppDbContext>()));

            services.AddScoped<IAuthenticate, AuthenticateServices>();

            var storagePath = configuration["Storage:BasePath"] ?? "uploads";
            services.AddScoped<IStorageService>(r => new StorageService(storagePath));

            return services;
        }
    }
}