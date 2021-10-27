// <copyright file = "IVehicleService.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework.Interfaces
{
    using System.Collections.Generic;
    using VacationHireInc.data.Entities;

    /// <summary>
    /// Contains functionality interacting with vehicles
    /// </summary>
    public interface IVehicleService
    {
        /// <summary>
        /// Gets a list of vehicles
        /// </summary>
        /// <returns>a list of vehicles</returns>
        List<Vehicle> GetVehicles();
    }
}
