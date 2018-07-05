﻿using MyCompany.MyProject.ApplicationCore;
using MyCompany.MyProject.ApplicationCore.SharedKernel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using System;

namespace MyCompany.MyProject.Web.Application
{
    public class GlobalExceptionHandleFilter : IAsyncExceptionFilter
    {
        readonly ILoggerFactory _loggerFactory;
        readonly IHostingEnvironment _env;
        readonly AppSettings _settings;

        public GlobalExceptionHandleFilter(ILoggerFactory loggerFactory, IHostingEnvironment env, IOptions<AppSettings> settings)
        {
            _loggerFactory = loggerFactory;
            _env = env;
            _settings = settings.Value;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var logger = _loggerFactory.CreateLogger(context.Exception.TargetSite.ReflectedType);

            if (context.Exception is DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                logger.LogWarning(dbUpdateConcurrencyException, "数据库存在并发问题");
                context.Result = new BadRequestObjectResult(new BadRequestObject("网络故障,请重试", dbUpdateConcurrencyException));
            }
            else if (context.Exception is AppException appException)
            {
                logger.LogInformation(appException.Message);
                context.Result = new BadRequestObjectResult(new BadRequestObject(appException.Message, appException.InnerException));
            }
            else
            {
                logger.LogError(
                    new EventId(_settings.EventId),
                    context.Exception,
                    context.Exception.Message);

                if (_env.IsProduction()) context.Result = new InternalServerErrorResult("未知错误,请重试");
                else context.Result = new InternalServerErrorResult(context.Exception);
            }

            context.ExceptionHandled = true;
            await Task.CompletedTask;
        }
    }

    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object value) : base(value)
        {
            StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    public class BadRequestObject
    {
        public BadRequestObject(string massage, Exception exception = null)
        {
            Massage = massage;
            Exception = exception;
        }

        public string Massage { get; private set; }

        public Exception Exception { get; private set; }
    }
}