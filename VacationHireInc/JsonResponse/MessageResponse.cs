// <copyright file="MessageResponse.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.JsonResponse
{
    /// <summary>
    /// JSON to be returned when making a store finder SMS response
    /// </summary>
    public class MessageResponse : SuccessfulResponseBase
    {
        /// <summary>
        /// Gets or sets a message to return back to the user indicating that the service is running
        /// </summary>
        public string Message { get; set; }
    }
}
