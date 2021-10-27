// <copyright file="GetOrdersResponse.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.JsonResponse
{
    using System.Collections.Generic;
    using VacationHireInc.data.Entities;

    /// <summary>
    /// Represents a response for GetOrders request
    /// </summary>
    public class GetOrdersResponse : SuccessfulResponseBase
    {
        /// <summary>
        /// Gets or sets a list HireOrder objects
        /// </summary>
        public IList<HireOrder> HireOrders { get; set; }
    }
}
