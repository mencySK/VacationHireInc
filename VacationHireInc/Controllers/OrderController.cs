// <copyright file="OrderController.cs" company="VacationHireInc">
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
    public class OrderController : Controller
    {
        /// <summary>
        /// An instance of the logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(OrderController));

        /// <summary>
        /// Instance of the order service
        /// </summary>
        private IOrderService orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">an instance of the order service</param>
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        /// <summary>
        /// Get requests used to create a new order
        /// </summary>
        /// <param name="model">model used for creating a new order</param>
        /// <returns>an appropriate message regarding the status of the request in JSON format</returns>
        [HttpGet("NewOrder")]
        [HttpPost("NewOrder")]
        public JsonResult NewOrder(OrderModel model)
        {
            IList<string> errors = model.Validate();

            if (string.IsNullOrEmpty(model.CustomerName))
            {
                errors.Add("Please enter the customer's name");
            }

            if (string.IsNullOrEmpty(model.CustomerPhoneNumber))
            {
                errors.Add("Please enter the customer's phone number");
            }

            foreach (string error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (!ModelState.IsValid)
            {
                Log.InfoFormat("Invalid order for vehicleID {0}", model.VehicleID);
                return this.Json(new ErrorResponse() { ErrorMessages = ModelState.Values.SelectMany(x => x.Errors).ToList().Select(x => x.ErrorMessage).ToList() });
            }

            string message = string.Empty;

            // If start date or end date is null, it would already return an error - this is used to convert from DateTime? to DateTime
            DateTime startDate = model.SubmittedStartDate ?? DateTime.Now;
            DateTime endDate = model.SubmittedEndDate ?? DateTime.Now;

            bool availability = this.orderService.InsertOrder(model.VehicleID, startDate, endDate, model.CustomerName, model.CustomerPhoneNumber, out message);

            if (availability)
            {
                return this.Json(new MessageResponse() { Message = message });
            }
            else
            {
                return this.Json(new ErrorResponse() { ErrorMessages = new List<string> { message } });
            }
        }

        /// <summary>
        /// Get requests used to cancel an order
        /// </summary>
        /// <param name="model">model used for cancelling an order</param>
        /// <returns>an appropriate message regarding the status of the request in JSON format</returns>
        [HttpGet("CancelOrder")]
        [HttpPost("CancelOrder")]
        public JsonResult CancelOrder(OrderModel model)
        {
            IList<string> errors = model.Validate();
            foreach (string error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (!ModelState.IsValid)
            {
                Log.InfoFormat("Invalid request for finishing order with vehicleID {0}", model.VehicleID);
                return this.Json(new ErrorResponse() { ErrorMessages = ModelState.Values.SelectMany(x => x.Errors).ToList().Select(x => x.ErrorMessage).ToList() });
            }

            string message = string.Empty;

            // If start date or end date is null, it would already return an error - this is used to convert from DateTime? to DateTime
            DateTime startDate = model.SubmittedStartDate ?? DateTime.Now;
            DateTime endDate = model.SubmittedEndDate ?? DateTime.Now;

            bool orderFound = this.orderService.CancelOrder(model.VehicleID, startDate, endDate, out message);

            if (orderFound)
            {
                return this.Json(new MessageResponse() { Message = message });
            }
            else
            {
                return this.Json(new ErrorResponse() { ErrorMessages = new List<string> { message } });
            }
        }

        /// <summary>
        /// Get requests used to finish an order
        /// </summary>
        /// <param name="model">model used for finishing an order</param>
        /// <returns>an appropriate message regarding the status of the request in JSON format</returns>
        [HttpGet("FinishOrder")]
        [HttpPost("FinishOrder")]
        public JsonResult FinishOrder(OrderModel model)
        {
            IList<string> errors = model.Validate();
            foreach (string error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (!ModelState.IsValid)
            {
                Log.InfoFormat("Invalid request for cancelling order with vehicleID {0}", model.VehicleID);
                return this.Json(new ErrorResponse() { ErrorMessages = ModelState.Values.SelectMany(x => x.Errors).ToList().Select(x => x.ErrorMessage).ToList() });
            }

            string message = string.Empty;

            // If start date or end date is null, it would already return an error - this is used to convert from DateTime? to DateTime
            DateTime startDate = model.SubmittedStartDate ?? DateTime.Now;
            DateTime endDate = model.SubmittedEndDate ?? DateTime.Now;

            bool orderFound = this.orderService.FinishOrder(model.VehicleID, startDate, endDate, model.Damage, model.GasolineFilled, out message);

            if (orderFound)
            {
                return this.Json(new MessageResponse() { Message = message });
            }
            else
            {
                return this.Json(new ErrorResponse() { ErrorMessages = new List<string> { message } });
            }
        }

        /// <summary>
        /// Get request used to display all orders
        /// </summary>
        /// <returns>a list of orders in JSON format</returns>
        [HttpGet("GetOrders")]
        [HttpPost("GetOrders")]
        public JsonResult GetOrders()
        {
            List<HireOrder> hireOrders = this.orderService.GetHireOrders();

            return this.Json(new GetOrdersResponse() { HireOrders = hireOrders });
        }
    }
}
