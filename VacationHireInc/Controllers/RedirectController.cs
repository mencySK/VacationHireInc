// <copyright file="RedirectController.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.webservice.Controllers
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Handles redirects
    /// </summary>
    [Route("{*url}")]
    public class RedirectController : Controller
    {
        /// <summary>
        /// Default page
        /// </summary>
        /// <returns>a url to redirect to</returns>
        public RedirectResult Index()
        {
            return this.Redirect("Server/Status");
        }
    }
}