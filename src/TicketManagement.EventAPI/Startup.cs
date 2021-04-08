using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Repository.EF;
using TicketManagement.EventAPI.Manager;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.EventAPI
{
#pragma warning disable S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    public class Startup
#pragma warning restore S1200 // Classes should not be coupled to too many other classes (Single Responsibility Principle)
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<EventService>();
            services.AddTransient<EventSeatService>();
            services.AddTransient<EventAreaService>();
            services.AddTransient<SeatService>();
            services.AddTransient<AreaService>();
            services.AddTransient<EventManager>();

            services.AddTransient<IRepository<EventData>, EventDataRepository>();
            services.AddTransient<IRepository<LayoutData>, LayoutRepository>();
            services.AddTransient<IRepository<Seat>, SeatRepository>();
            services.AddTransient<IRepository<Area>, AreaRepository>();
            services.AddTransient<IRepository<EventSeat>, EventSeatRepository>();
            services.AddTransient<IRepository<EventArea>, EventAreaRepository>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("Jwt:JwtSecretKey").Value)),
                            ValidateIssuer = true,
                            ValidIssuer = Configuration.GetSection("Jwt:JwtIssuer").Value,
                            ValidateAudience = true,
                            ValidAudience = Configuration.GetSection("Jwt:JwtAudience").Value,
                        };
                    });

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketManagement.EventAPI", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "Jwt Token is required to access the endpoints",
                    In = ParameterLocation.Header,
                    Name = "JWT Authentication",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    },
                };
                options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() },
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketManagement.EventAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

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
