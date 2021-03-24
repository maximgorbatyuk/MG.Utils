using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MG.Utils.AspNetCore.Middlewares.Error
{
    public abstract class ValidationProblemDetailsResultBase : IActionResult
    {
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var modelStateEntries = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToArray();

            var errors = new List<ValidationError>();

            if (modelStateEntries.Any())
            {
                foreach (var (key, value) in modelStateEntries)
                {
                    errors.AddRange(value.Errors
                        .Select(modelStateError => new ValidationError(
                            name: Serialize(key),
                            description: modelStateError.ErrorMessage)));
                }
            }

            await new JsonErrorResponse(
                context: context.HttpContext,
                serializedError: Serialize(new ValidationProblemDetails(errors, AppInstanceName)),
                statusCode: ValidationProblemDetails.ValidationStatusCode)
                .WriteAsync();
        }

        public abstract string AppInstanceName { get; }

        public abstract string Serialize<T>([NotNull] T instance);

        public virtual string Serialize([NotNull] string key)
        {
            return key;
        }
    }
}