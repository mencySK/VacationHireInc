// <copyright file="Startup.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using VacationHireInc.data.Entities;
    using VacationHireInc.framework;
    using VacationHireInc.framework.Interfaces;
    using VacationHireInc.framework.Services;

    /// <summary>
    /// Class used to configure the web application when it is started
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">the environment the application is to be hosted upon</param>
        public Startup(IWebHostEnvironment env)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead($"log4net.{env.EnvironmentName}.config"));
            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        /// <summary>
        /// Gets an instance of the configuration interface
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Called at runtime to set up the services needed to run the web application
        /// </summary>
        /// <param name="services">a collection of services to set up</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddScoped<ICredentialsService, CredentialsService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IVehicleService, VehicleService>();

            AppSettings settings = new AppSettings(this.Configuration);

            ICredentialsService credentialsService = new CredentialsService(this.Configuration);

            string connectionString = string.Empty;
            var getConnectionString = Task.Run(async () => connectionString = await credentialsService.GetCredentials(settings.ConnectionStringLocation));
            getConnectionString.Wait();

            services.AddDbContext<Repository>(options => options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Sets up the http request pipeline for the application
        /// </summary>
        /// <param name="app">an instance of the application to host</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Error");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Redirect}/{action=Index}");
            });
        }
    }
}
