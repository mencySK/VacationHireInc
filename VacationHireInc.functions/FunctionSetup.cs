// <copyright file="FunctionSetup.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.functions
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Xml;
    using Microsoft.Azure.WebJobs;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using log4net;
    using VacationHireInc.framework.Interfaces;
    using VacationHireInc.data.Entities;
    using VacationHireInc.framework;

    /// <summary>
    /// Contains configuration set up for functions
    /// </summary>
    public static class FunctionSetup
    {
        /// <summary>
        /// Builds a log 4 net object from a configuration file
        /// </summary>
        /// <param name="context">execution context</param>
        /// <returns>a logger to be used by the functions</returns>
        public static ILog SetupLogging(ExecutionContext context)
        {
            string environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT_LEVEL");
            var fileContents = File.ReadAllText(Path.Combine(context.FunctionAppDirectory, string.Format("log4net.{0}.config", environmentName)));
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileContents);
            var repo = LogManager.CreateRepository(
               Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, doc["log4net"]);

            ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            return log;
        }

        /// <summary>
        /// Pulls a connection string from a configurable store and builds a repository
        /// </summary>
        /// <param name="context">the applications hosting context</param>
        /// <returns>a repository</returns>
        public static Repository GetRepository(ExecutionContext context)
        {
            var config = GetConfiguration(context);

            AppSettings settings = new AppSettings(config);
            ICredentialsService credentialsService = new CredentialsService(config);

            string connectionString = string.Empty;
            var getConnectionString = Task.Run(async () => connectionString = await credentialsService.GetCredentials(settings.ConnectionStringLocation));
            getConnectionString.Wait();

            var options = new DbContextOptionsBuilder<Repository>();
            options.UseSqlServer(connectionString);
            return new Repository(options.Options);
        }

        /// <summary>
        /// Creates a new configuration object based on the app settings
        /// </summary>
        /// <param name="context">the current context in which the application is being executed</param>
        /// <returns>a configuration object </returns>
        public static IConfiguration GetConfiguration(ExecutionContext context)
        {
            string environmentName = Environment.GetEnvironmentVariable("ENVIRONMENT_LEVEL");
            return new ConfigurationBuilder()
        .SetBasePath(context.FunctionAppDirectory)
        .AddJsonFile(string.Format("appsettings.{0}.json", environmentName), optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
        }
    }
}
