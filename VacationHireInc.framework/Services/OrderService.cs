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
    using VacationHireInc.webservice.Models;
    using System.Net.Http;
    using System.Threading.Tasks;

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
        /// <param name="model">order object</param>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        public bool InsertOrder(OrderModel model, out string message)
        {
            message = string.Empty;

            // Check if the submitted vehicle id coresponds to any existing vehicle
            Vehicle vehicle = this.repository.Vehicles.Where(x => x.Id == model.VehicleID).FirstOrDefault();
            if (vehicle == null)
            {
                message = "This vehicle does not exist";
                Log.InfoFormat("Vehicle {0} attempt to hire but submitted vehicle id does not corespond to any existing vehicle", model.VehicleID);
                return false;
            }

            // Check if the vehicle is already booked, i am using a list as there could potentially be multiple bookings for the same period but have been cancelled
            List<HireOrder> existingHireOrders = this.repository.HireOrders.Where(x => x.VehicleId == model.VehicleID &&
                ((model.SubmittedStartDate >= x.StartDate && model.SubmittedStartDate <= x.EndDate) || (model.SubmittedEndDate >= x.StartDate && model.SubmittedEndDate <= x.EndDate) ||
                (model.SubmittedStartDate <= x.StartDate && model.SubmittedEndDate >= x.EndDate)) && x.Status != OrderStatus.Cancelled).ToList();

            // If the vehicle is not available for the requested dates, return an appropiate message
            if (existingHireOrders.Count > 0)
            {
                message = "This vehicle is not available for the selected dates";
                Log.InfoFormat("Vehicle {0} attempted to hire but there is already a pending booking for the selected dates", model.VehicleID);
                return false;
            }
            else
            {
                HireOrder newHireOrder = new HireOrder();
                newHireOrder.Id = Guid.NewGuid();
                newHireOrder.Status = OrderStatus.Booked;
                newHireOrder.StartDate = (DateTime)model.SubmittedStartDate;
                newHireOrder.EndDate = (DateTime)model.SubmittedEndDate;
                newHireOrder.CustomerName = model.CustomerName;
                newHireOrder.CustomerPhoneNumber = model.CustomerPhoneNumber;
                newHireOrder.VehicleId = model.VehicleID;

                message = string.Format("Vehicle successfully booked between {0} and {1}", model.StartDate, model.EndDate);
                this.repository.HireOrders.Add(newHireOrder);
                this.repository.SaveChanges();
                Log.InfoFormat("New order made for vehicle {0}", model.VehicleID);
                return true;
            }
        }

        /// <summary>
        /// Mark an order as cancelled
        /// </summary>
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        public bool CancelOrder(OrderModel model, out string message)
        {
            HireOrder hireOrder = this.repository.HireOrders.Where(x => x.VehicleId == model.VehicleID && x.StartDate == model.SubmittedStartDate && x.EndDate == model.SubmittedEndDate && x.Status == OrderStatus.Booked).FirstOrDefault();
            if (hireOrder != null)
            {
                hireOrder.Status = OrderStatus.Cancelled;
                this.repository.Update<HireOrder>(hireOrder);
                this.repository.SaveChanges();
                Log.InfoFormat("Order with VehicleID {0} and start date {1} and end date {2} successfully cancelled", model.VehicleID, model.StartDate, model.EndDate);
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
        /// <param name="message">message to be returned</param>
        /// <returns>returns a message describing the status of the request</returns>
        public bool FinishOrder(OrderModel model, out string message)
        {
            HireOrder hireOrder = this.repository.HireOrders.Where(x => x.VehicleId == model.VehicleID && x.StartDate == model.SubmittedStartDate && x.EndDate == model.SubmittedEndDate && x.Status == OrderStatus.Processing).FirstOrDefault();
            if (hireOrder != null)
            {
                hireOrder.Status = OrderStatus.Finished;
                hireOrder.Damage = model.Damage;
                hireOrder.GasolineFilled = model.GasolineFilled;
                this.repository.Update<HireOrder>(hireOrder);
                this.repository.SaveChanges();
                message = "Order successfully finished";
                Log.InfoFormat("Order with VehicleID {0} and start date {1} and end date {2} successfully finished", model.VehicleID, model.StartDate, model.EndDate);
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

        /// <summary>
        /// Makes an asynchronous web request to a url and returns the result
        /// </summary>
        /// <param name="url">the url to call</param>
        /// <param name="header">any header values to add</param>
        /// <returns>the result of the web request</returns>
        public async Task<string> GetWebRequest(string url)
        {
            HttpClient httpClient = new HttpClient();
            Log.InfoFormat("Making get request to address {0}", url);
            return await httpClient.GetStringAsync(url);
        }
    }
}
