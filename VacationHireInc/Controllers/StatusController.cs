// <copyright file="StatusController.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using VacationHireInc.webservice.JsonResponse;

    /// <summary>
    /// Contains items relating to getting the current status of the service
    /// </summary>
    [Produces("application/json")]
    [Route("Server")]
    public class StatusController : Controller
    {
        /// <summary>
        /// Returns an ok status to indicate that the service is online and running
        /// </summary>
        /// <returns>A JSON response</returns>
        [HttpGet("Status")]
        [HttpPost("Status")]
        public JsonResult GetStatus()
        {
            return this.Json(new MessageResponse() { Message = "STATUS=OK" });
        }
    }
}