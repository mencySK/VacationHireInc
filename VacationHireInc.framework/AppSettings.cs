// <copyright file = "AppSettings.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Contains application settings loaded from the app settings JSON file
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The configuration object upon which the app settings are based
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettings"/> class.
        /// </summary>
        /// <param name="configuration">the configuration settings to return</param>
        public AppSettings(IConfiguration configuration)
        {
            this.config = configuration;
        }

        /// <summary>
        /// Gets the location of the vault the connection string is stored in
        /// </summary>
        public string ConnectionStringLocation
        {
            get
            {
                return this.config.GetValue<string>("Values:ConnectionStringLocation");
            }
        }

        /// <summary>
        /// Gets the location of the azure vault holding passwords for the current environment
        /// </summary>
        public string AzureVault
        {
            get
            {
                return this.config.GetValue<string>("Values:AzureVault");
            }
        }
    }
}
