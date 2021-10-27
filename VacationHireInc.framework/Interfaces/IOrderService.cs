// <copyright file = "IOrderService.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.framework.Interfaces
{
    using System;
    using System.Collections.Generic;
    using VacationHireInc.data.Entities;

    /// <summary>
    /// Contains functionality interacting with orders
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Create a new order/booking
        /// </summary>
        /// <param name="vehicleID">the vehicle id to make the order for</param>
        /// <param name="startDate">start date of the booking</param>
        /// <param name="endDate">end date of the booking</param>
        /// <param name="customerName">customer's name booking the vehicle</param>
        /// <param name="customerPhoneNumber">customer's phone number booking the vehicle</param>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        bool InsertOrder(Guid vehicleID, DateTime startDate, DateTime endDate, string customerName, string customerPhoneNumber, out string message);

        /// <summary>
        /// Mark an order as cancelled
        /// </summary>
        /// <param name="vehicleID">vehicle id used to search for the booking to be cancelled</param>
        /// <param name="startDate">start date used to search for the booking to be cancelled</param>
        /// <param name="endDate">end date used to search for the booking to be cancelled</param>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        bool CancelOrder(Guid vehicleID, DateTime startDate, DateTime endDate, out string message);

        /// <summary>
        /// Mark an order as finished/processed
        /// </summary>
        /// <param name="vehicleID">vehicle id for the order/booking to finish</param>
        /// <param name="startDate">start date used to search for the booking to finish</param>
        /// <param name="endDate">end date used to search for the booking to finish</param>
        /// <param name="damage">damage description if any</param>
        /// <param name="gasolineFilled">true or false indicating whether the tank is filled or not</param>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        bool FinishOrder(Guid vehicleID, DateTime startDate, DateTime endDate, string damage, bool gasolineFilled, out string message);

        /// <summary>
        /// Gets a list of orders
        /// </summary>
        /// <returns>a list of orders</returns>
        List<HireOrder> GetHireOrders();
    }
}
