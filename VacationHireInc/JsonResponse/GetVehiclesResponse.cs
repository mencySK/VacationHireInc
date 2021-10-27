// <copyright file="GetVehiclesResponse.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.JsonResponse
{
    using System.Collections.Generic;
    using VacationHireInc.data.Entities;

    /// <summary>
    /// Represents a response for GetVehicle request
    /// </summary>
    public class GetVehiclesResponse : SuccessfulResponseBase
    {
        /// <summary>
        /// Gets or sets a list Vehicle objects
        /// </summary>
        public IList<Vehicle> vehicles { get; set; }
    }
}
