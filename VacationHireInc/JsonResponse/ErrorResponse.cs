// <copyright file="ErrorResponse.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.JsonResponse
{
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// Represents an error returned by the application
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets a list of error messages 
        /// </summary>
        public IList<string> ErrorMessages { get; set; }

        /// <summary>
        /// Gets a value indicating whether an error has occurred
        /// </summary>
        public bool SuccessfulRequest
        {
            get
            {
                return false;
            }
        }

        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
