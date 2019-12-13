using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using RegistrationApp.Domain.Interfaces;
using RegistrationApp.Domain.Interfaces.Repositories;
using RegistrationApp.Domain.Interfaces.Repositories.Identity;
using RegistrationApp.Domain.Interfaces.Services;
using RegistrationApp.Domain.Interfaces.Services.Identity;
using RegistrationApp.Domain.Services;
using RegistrationApp.Domain.Services.Identity;
using RegistrationApp.Persistence;
using RegistrationApp.Persistence.Context;
using RegistrationApp.Persistence.Repositories;
using RegistrationApp.Persistence.Repositories.Identity;
using System.Text;
using AutoMapper;
using RegistrationApp.Api.Utilities.Authentication;

namespace RegistrationApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            var authenticationSection = Configuration.GetSection("Authentication");

            services.AddSingleton(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connection));

            services.Configure<AuthenticationParameters>(authenticationSection);

            services.AddTransient<JwtTokenGeneratorForAuthentication>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITodoListService, TodoListService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var jwtSecretString = authenticationSection.Get<AuthenticationParameters>().JwtSecretString;
            var key = Encoding.ASCII.GetBytes(jwtSecretString);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
