// <copyright file="VehicleController.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using VacationHireInc.data.Entities;
    using VacationHireInc.framework.Interfaces;
    using VacationHireInc.webservice.JsonResponse;
    using VacationHireInc.webservice.Models;

    /// <summary>
    /// Handles calls made to the service to manipulate orders
    /// </summary>
    [Route("[Controller]")]
    public class VehicleController : Controller
    {
        /// <summary>
        /// An instance of the logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(VehicleController));

        /// <summary>
        /// Instance of the order service
        /// </summary>
        private IVehicleService vehicleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleController"/> class.
        /// </summary>
        /// <param name="vehicleService">an instance of the vehicle service</param>
        public VehicleController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        /// <summary>
        /// Get request used to display all vehicles
        /// </summary>
        /// <returns>a list of orders in JSON format</returns>
        [HttpGet("GetVehicles")]
        [HttpPost("GetVehicles")]
        public JsonResult GetVehicles()
        {
            List<Vehicle> vehicles = this.vehicleService.GetVehicles();

            return this.Json(new GetVehiclesResponse() { vehicles = vehicles });
        }
    }
}
