// <copyright file = "CredentialsService.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Configuration;
    using VacationHireInc.framework;
    using VacationHireInc.framework.Interfaces;

    /// <summary>
    /// Implements the <see cref="ICredentialsService"/> interface
    /// </summary>
    public class CredentialsService : ICredentialsService
    {
        /// <summary>
        /// An instance of the application settings class
        /// </summary>
        private AppSettings appSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CredentialsService"/> class.
        /// </summary>
        /// <param name="config">the application settings to use</param>
        public CredentialsService(IConfiguration config)
        {
            this.appSettings = new AppSettings(config);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetCredentials(string credentialStore)
        {
            SecretBundle bundle = new SecretBundle();
            try
            {
                AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
                KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                bundle = await keyVaultClient.GetSecretAsync(string.Format("{0}/secrets/{1}", this.appSettings.AzureVault, credentialStore)).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return null;
            }

            return bundle.Value;
        }
    }
}
