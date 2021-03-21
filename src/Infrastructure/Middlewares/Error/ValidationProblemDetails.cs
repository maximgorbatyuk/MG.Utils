using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Utils.Helpers;

namespace WebHost.Infrastructure.Middlewares.Error
{
    public class ValidationProblemDetails : ProblemDetails
    {
        public const int ValidationStatusCode = (int)HttpStatusCode.BadRequest;

        public ValidationProblemDetails(ICollection<ValidationError> validationErrors)
        {
            validationErrors.ThrowIfNullOrEmpty(nameof(validationErrors));
            ValidationErrors = validationErrors;

            Status = ValidationStatusCode;
            Title = "Request Validation Error";

            // TODO Maxim: если будет балансировка, то тут нужно будет передавать имя ноды
            Instance = "CT Portal";
        }

        public ICollection<ValidationError> ValidationErrors { get; }

        public string RequestId => Guid.NewGuid().ToString();
    }
}