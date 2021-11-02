// <copyright file = "IOrderService.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VacationHireInc.data.Entities;
    using VacationHireInc.webservice.Models;

    /// <summary>
    /// Contains functionality interacting with orders
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Create a new order/booking
        /// </summary>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        bool InsertOrder(OrderModel model, out string message);

        /// <summary>
        /// Mark an order as cancelled
        /// </summary>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        bool CancelOrder(OrderModel model, out string message);

        /// <summary>
        /// Mark an order as finished/processed
        /// </summary>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        bool FinishOrder(OrderModel model, out string message);

        /// <summary>
        /// Gets a list of orders
        /// </summary>
        /// <returns>a list of orders</returns>
        List<HireOrder> GetHireOrders();

        /// <summary>
        /// Makes a web request
        /// </summary>
        /// <param name="url">url to send the request</param>
        /// <returns></returns>
        Task<string> GetWebRequest(string url);
    }
}
