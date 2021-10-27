// <copyright file = "OrderService.cs" company="VacationHireInc">
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
    /// Implementation of the order interface
    /// </summary>
    public class OrderService : IOrderService
    {
        /// <summary>
        /// An instance of the logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(OrderService));

        /// <summary>
        /// An instance of the repository used to interact with the database
        /// </summary>
        private Repository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="repository">the repository to use</param>
        public OrderService(Repository repository)
        {
            this.repository = repository;
        }

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
        public bool InsertOrder(Guid vehicleID, DateTime startDate, DateTime endDate, string customerName, string customerPhoneNumber, out string message)
        {
            message = string.Empty;

            // Check if the submitted vehicle id coresponds to any existing vehicle
            Vehicle vehicle = this.repository.Vehicles.Where(x => x.Id == vehicleID).FirstOrDefault();
            if (vehicle == null)
            {
                message = "This vehicle does not exist";
                Log.InfoFormat("Vehicle {0} attempt to hire but submitted vehicle id does not corespond to any existing vehicle", vehicleID);
                return false;
            }

            // Check if the vehicle is already booked, i am using a list as there could potentially be multiple bookings for the same period but have been cancelled
            List<HireOrder> existingHireOrders = this.repository.HireOrders.Where(x => x.VehicleId == vehicleID &&
                ((startDate >= x.StartDate && startDate <= x.EndDate) || (endDate >= x.StartDate && endDate <= x.EndDate) ||
                (startDate <= x.StartDate && endDate >= x.EndDate)) && x.Status != OrderStatus.Cancelled).ToList();

            // If the vehicle is not available for the requested dates, return an appropiate message
            if (existingHireOrders.Count > 0)
            {
                message = "This vehicle is not available for the selected dates";
                Log.InfoFormat("Vehicle {0} attempted to hire but there is already a pending booking for the selected dates", vehicleID);
                return false;
            }
            else
            {
                HireOrder newHireOrder = new HireOrder();
                newHireOrder.Id = Guid.NewGuid();
                newHireOrder.Status = OrderStatus.Booked;
                newHireOrder.StartDate = startDate;
                newHireOrder.EndDate = endDate;
                newHireOrder.CustomerName = customerName;
                newHireOrder.CustomerPhoneNumber = customerPhoneNumber;
                newHireOrder.VehicleId = vehicleID;

                message = string.Format("Vehicle successfully booked between {0} and {1}", startDate, endDate);
                this.repository.HireOrders.Add(newHireOrder);
                this.repository.SaveChanges();
                Log.InfoFormat("New order made for vehicle {0}", vehicleID);
                return true;
            }
        }

        /// <summary>
        /// Mark an order as cancelled
        /// </summary>
        /// <param name="vehicleID">vehicle id used to search for the booking to be cancelled</param>
        /// <param name="startDate">start date used to search for the booking to be cancelled</param>
        /// <param name="endDate">end date used to search for the booking to be cancelled</param>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        public bool CancelOrder(Guid vehicleID, DateTime startDate, DateTime endDate, out string message)
        {
            HireOrder hireOrder = this.repository.HireOrders.Where(x => x.VehicleId == vehicleID && x.StartDate == startDate && x.EndDate == endDate && x.Status == OrderStatus.Booked).FirstOrDefault();
            if (hireOrder != null)
            {
                hireOrder.Status = OrderStatus.Cancelled;
                this.repository.Update<HireOrder>(hireOrder);
                this.repository.SaveChanges();
                Log.InfoFormat("Order with VehicleID {0} and start date {1} and end date {2} successfully cancelled", vehicleID, startDate, endDate);
                message = "Order successfully cancelled";
                return true;
            }
            else
            {
                message = "Order not found or already started";
                return false;
            }
        }

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
        public bool FinishOrder(Guid vehicleID, DateTime startDate, DateTime endDate, string damage, bool gasolineFilled, out string message)
        {
            HireOrder hireOrder = this.repository.HireOrders.Where(x => x.VehicleId == vehicleID && x.StartDate == startDate && x.EndDate == endDate && x.Status == OrderStatus.Processing).FirstOrDefault();
            if (hireOrder != null)
            {
                hireOrder.Status = OrderStatus.Finished;
                hireOrder.Damage = damage;
                hireOrder.GasolineFilled = gasolineFilled;
                this.repository.Update<HireOrder>(hireOrder);
                this.repository.SaveChanges();
                message = "Order successfully finished";
                Log.InfoFormat("Order with VehicleID {0} and start date {1} and end date {2} successfully finished", vehicleID, startDate, endDate);
                return true;
            }
            else
            {
                message = "Order not found for submitted details";
                return false;
            }
        }

        /// <summary>
        /// Used by the UpdateOrdersStatus function in order to automatically set the status of orders
        /// </summary>
        public void UpdateOrdersStatus()
        {
            List<HireOrder> ordersToMarkAsProcessing = this.repository.HireOrders.Where(x => x.Status == OrderStatus.Booked && x.StartDate > DateTimeHelper.ServerTime() && x.EndDate < DateTimeHelper.ServerTime()).ToList();
            List<HireOrder> ordersToMarkAsFinished = this.repository.HireOrders.Where(x => x.Status == OrderStatus.Processing && x.EndDate < DateTimeHelper.ServerTime()).ToList();

            foreach (HireOrder hireOrder in ordersToMarkAsProcessing)
            {
                hireOrder.Status = OrderStatus.Processing;
                this.repository.Update<HireOrder>(hireOrder);
            }

            foreach (HireOrder hireOrder in ordersToMarkAsFinished)
            {
                hireOrder.Status = OrderStatus.Finished;
                hireOrder.GasolineFilled = true;
                this.repository.Update<HireOrder>(hireOrder);
            }

            this.repository.SaveChanges();
        }

        /// <summary>
        /// Gets a list of orders
        /// </summary>
        /// <returns>a list of orders</returns>
        public List<HireOrder> GetHireOrders()
        {
            return this.repository.HireOrders.ToList();
        }
    }
}
