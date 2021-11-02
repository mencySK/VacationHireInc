// <copyright file="ErrorController.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

using System.Collections.Generic;
using VacationHireInc.webservice.JsonResponse;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

/// <summary>
/// Log unhandled errors
/// </summary>
[Produces("application/json")]
[Route("[Controller]")]
public class ErrorController : Controller
{
    /// <summary>
    /// An instance of the logger
    /// </summary>
    private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ErrorController));

    /// <summary>
    /// Log unhandled errors        
    /// </summary>
    /// <returns>return message to user</returns>
    public JsonResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        Log.Error("An error has occured at path: " + exceptionHandlerPathFeature.Path, exceptionHandlerPathFeature.Error);

         return this.Json(new ErrorResponse() { ErrorMessages = new List<string> { "An error has occured" }, HttpStatusCode = HttpStatusCode.InternalServerError });
    }
} 