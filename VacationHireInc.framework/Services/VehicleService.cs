// <copyright file = "VehicleService.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VacationHireInc.data.Entities;
    using VacationHireInc.framework.Interfaces;
    using VacationHireInc.framework.Helpers;

    /// <summary>
    /// Implementation of the vehicle interface
    /// </summary>
    public class VehicleService : IVehicleService
    {
        /// <summary>
        /// An instance of the logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(VehicleService));

        /// <summary>
        /// An instance of the repository used to interact with the database
        /// </summary>
        private Repository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleService"/> class.
        /// </summary>
        /// <param name="repository">the repository to use</param>
        public VehicleService(Repository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets a list of vehicles
        /// </summary>
        /// <returns>a list of vehicles</returns>
        public List<Vehicle> GetVehicles()
        {
            return this.repository.Vehicles.ToList();
        }
    }
}
