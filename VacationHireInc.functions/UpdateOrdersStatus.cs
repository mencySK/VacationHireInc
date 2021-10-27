// <copyright file="UpdateOrdersStatus.cs" company="VacationHireInc">
// Copyright (c) 2021 All Rights Reserved
// </copyright>

namespace VacationHireInc.functions
{
    using System;
    using log4net;
    using Microsoft.Azure.WebJobs;
    using VacationHireInc.data.Entities;
    using VacationHireInc.framework.Services;

    /// <summary>
    /// Function that runs hourly to update orders status
    /// </summary>
    public static class UpdateOrdersStatus
    {
        /// <summary>
        /// Runs through each order and updates the status depending if it has started or finished
        /// </summary>
        /// <param name="myTimer">the timer</param>
        /// <param name="context">Details of the environment the request is being run in</param>
        [FunctionName("UpdateOrdersStatus")]
        public static void Run([TimerTrigger("0 * */1 * * *", RunOnStartup = true)]TimerInfo myTimer, ExecutionContext context)
        {
            ILog log = FunctionSetup.SetupLogging(context);
            var config = FunctionSetup.GetConfiguration(context);
            log.Info($"Update orders status function started at: {DateTime.Now}");
            Repository repository = FunctionSetup.GetRepository(context);
            OrderService orderService = new OrderService(repository);
            orderService.UpdateOrdersStatus();
            log.Info($"Update orders status function finished at: {DateTime.Now}");
        }
    }
}
