// <copyright file="ICredentialsService.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Stores functionality for settings and receiving hidden credentials
    /// </summary>
    public interface ICredentialsService
    {
        /// <summary>
        /// Connects to the environments azure vault and pulls out a secret
        /// </summary>
        /// <param name="credentialStore">the name of the secret to get</param>
        /// <returns>a password held in the azure vault</returns>
        Task<string> GetCredentials(string credentialStore);
    }
}
