﻿using System;

namespace MG.Utils.AspNetCore.Middlewares.Error
{
    public class ErrorDetails
    {
        private const string DefaultServerErrorMessage = "Internal Server Error";

        public ErrorDetails(int status, string message = null)
        {
            Status = status;
            Message = message ?? DefaultServerErrorMessage;
        }

        public int Status { get; }

        public string Message { get; }

        public string RequestId => Guid.NewGuid().ToString();
    }
}