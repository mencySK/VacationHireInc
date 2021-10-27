// <copyright file="Program.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    /// <summary>
    /// The entry point of the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the application
        /// </summary>
        /// <param name="args">arguments to be passed through when starting the service</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds a web application
        /// </summary>
        /// <param name="args">the arguments to be used</param>
        /// <returns>a hosted web application</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
