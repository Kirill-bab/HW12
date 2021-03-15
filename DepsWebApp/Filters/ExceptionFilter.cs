using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsWebApp.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private IReadOnlyDictionary<Type, (int, string)> _exceptions;
        public ExceptionFilter(ILoggerFactory loggerFactory, RegistrationExceptions exceptions)
        {
            _logger = loggerFactory.CreateLogger<ExceptionFilter>();
            _exceptions = exceptions.Exceptions;
        }
        public void OnException(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();
            (int, string) exception;
            if (!_exceptions.ContainsKey(exceptionType))
            {
                exception = (666, "unexpected exception occured!");
            }
            else
            {
                exception = _exceptions[context.Exception.GetType()];
            }

            context.Result = new ContentResult
            {
                Content = JsonSerializer.Serialize(
                   new
                   {
                       code = exception.Item1,
                       message = exception.Item2
                   }, new JsonSerializerOptions
                   {
                       WriteIndented = true
                   })
            };
            context.ExceptionHandled = true;
        }
    }
}
