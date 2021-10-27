// <copyright file = "OrderModel.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using VacationHireInc.framework.Helpers;

    /// <summary>
    /// Model for creating, cancelling or finishing an order
    /// </summary>
    public class OrderModel
    {
        /// <summary>
        /// Gets or sets the vehicle id
        /// </summary>
        [Required(ErrorMessage = "Please enter the VehicleID")]
        public Guid VehicleID { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        [Required(ErrorMessage = "Please enter the start date of the booking")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date
        /// </summary>
        [Required(ErrorMessage = "Please enter the end date of the booking")]
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the customer's name
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customer's phone number
        /// </summary>
        public string CustomerPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the damage if any
        /// </summary>
        public string Damage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tank is filled or not
        /// </summary>
        public bool GasolineFilled { get; set; }

        /// <summary>
        /// Gets or sets converted start date
        /// </summary>
        public DateTime? SubmittedStartDate { get; set; }

        /// <summary>
        /// Gets or sets converted end date
        /// </summary>
        public DateTime? SubmittedEndDate { get; set; }

        /// <summary>
        /// validate and convert the date
        /// </summary>
        /// <returns>the converted date</returns>
        public IList<string> Validate()
        {
            IList<string> errors = new List<string>();
            DateTime? convertedStartDate = null;
            DateTime? convertedEndDate = null;

            DateTime? startDateOut;
            DateTime? endDateOut;

            bool formatCheckStartDatetime = DateTimeHelper.FullDateTimeConversion(this.StartDate, out convertedStartDate);
            if (!formatCheckStartDatetime)
            {
                formatCheckStartDatetime = DateTimeHelper.DateConversion(this.StartDate, out startDateOut);
                convertedStartDate = startDateOut;
            }

            bool formatCheckEndDatetime = DateTimeHelper.FullDateTimeConversion(this.EndDate, out convertedEndDate);
            if (!formatCheckEndDatetime)
            {
                formatCheckEndDatetime = DateTimeHelper.DateConversion(this.EndDate, out endDateOut);
                convertedEndDate = endDateOut;
            }

            if ((!formatCheckStartDatetime) || (convertedStartDate < (DateTime)SqlDateTime.MinValue) || (convertedStartDate > (DateTime)SqlDateTime.MaxValue))
            {
                if (this.StartDate != null)
                {
                    errors.Add("Invalid start date. Please use this format: yyyy-MM-dd HH:mm:ss");
                }
            }

            if ((!formatCheckEndDatetime) || (convertedEndDate < (DateTime)SqlDateTime.MinValue) || (convertedEndDate > (DateTime)SqlDateTime.MaxValue))
            {
                if (this.EndDate != null)
                {
                    errors.Add("Invalid end date. Please use this format: yyyy-MM-dd HH:mm:ss");
                }
            }
            else if (convertedEndDate < convertedStartDate)
            {
                errors.Add("Invalid Request. Attempt made with end date earlier than start date");
            }

            this.SubmittedStartDate = convertedStartDate;
            this.SubmittedEndDate = convertedEndDate;

            return errors;
        }
    }
}
