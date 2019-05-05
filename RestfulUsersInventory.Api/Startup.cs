using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestfulUsersInventory.DataAccess;
using RestfulUsersInventory.DataQueries;
using RestfulUsersInventory.DataQueries.DTOs;
using System;
using System.IO;
using System.Reflection;

namespace RestfulUsersInventory.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // EF Database and Queries
            string connectionString = _configuration.GetConnectionString("RestfulUsersInventoryDb");
            if (connectionString.Contains("%DATABASEROUTEPATH%"))
            {
                string dbFolderPath = Path.Combine
                (
                    Environment.CurrentDirectory,
                    "..\\RestfulUsersInventory.Database"
                );
                connectionString = connectionString.Replace("%DATABASEROUTEPATH%", dbFolderPath);
            }
            services.AddDbContext<ApplicationDbContext>
            (
                options => options
                    .UseSqlite
                    (
                        connectionString,
                        // DbContext is in different assembly
                        builder => builder.MigrationsAssembly("RestfulUsersInventory.DataAccess")
                    )
            );
            services.AddTransient<IQueryHelper, QueryHelper>();

            // AutoMapper
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // MVC
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(Routes.ConfigureRoutes);
        }
    }
}
