// <copyright file="SuccessfulResponseBase.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

using System.Net;

namespace VacationHireInc.webservice.JsonResponse
{
    /// <summary>
    /// basic successful response
    /// </summary>
    public class SuccessfulResponseBase
    {
        /// <summary>
        /// Gets a value indicating whether an error has occurred
        /// </summary>
        public bool SuccessfulRequest
        {
            get
            {
                return true;
            }
        }

        public HttpStatusCode HttpStatusCode
        {
            get
            {
                return HttpStatusCode.OK;
            }
        }
    }
}
