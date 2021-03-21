using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Logging;

namespace WebHost.Infrastructure.Contracts.ScheduleJobs
{
    public abstract class InvocableJobBase : IInvocable
    {
        private readonly ILogger _logger;

        private string JobName => GetType().Name;

        protected InvocableJobBase(ILogger logger)
        {
            _logger = logger;
        }

        protected abstract Task InvokeAsync();

#pragma warning disable UseAsyncSuffix // Use Async suffix
        public async Task Invoke()
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            Stopwatch stopwatch = null;
            try
            {
                stopwatch = Stopwatch.StartNew();

                await InvokeAsync();

                stopwatch.Stop();

                _logger.LogInformation(
                    $"Finished background task {JobName}. Execution time: {stopwatch.ElapsedMilliseconds}");
            }
            catch (Exception exception)
            {
                stopwatch?.Stop();

                _logger.LogError(
                    exception,
                    $"A task {JobName} has error: {exception.Message}");
            }
        }
    }
}