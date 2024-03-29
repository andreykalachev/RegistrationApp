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

namespace RegistrationApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddSingleton(Configuration);

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITodoListRepository, TodoListRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITodoListService, TodoListService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var secretString = "MySecretString";
            var key = Encoding.ASCII.GetBytes(secretString);
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

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
