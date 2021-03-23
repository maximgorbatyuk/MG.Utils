using System;
using System.ComponentModel.DataAnnotations;
using MG.Utils.AspNetCore.Validation;
using MG.Utils.I18N;
using Microsoft.AspNetCore.Http;

namespace MG.Utils.AspNetCore.Attributes
{
    // copied from https://stackoverflow.com/questions/56588900/how-to-validate-uploaded-file-in-asp-net-core
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private const int Kilo = 1024;

        // In bytes
        private readonly int _maxFileSizeInBytes;
        private readonly int _maxSizeInMegabytes;

        public MaxFileSizeAttribute(int megabytes)
        {
            _maxSizeInMegabytes = megabytes;
            _maxFileSizeInBytes = megabytes * Kilo * Kilo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            return value switch
            {
                IFormFile file => file.Length > _maxFileSizeInBytes
                    ? new ValidationResult(validationContext.ErrorMessage(ErrorMessage ?? DataAnnotationErrorMessages.InvalidMaxFileSize, _maxSizeInMegabytes))
                    : ValidationResult.Success,
                _ => throw new InvalidOperationException("Use this attribute only for file properties")
            };
        }
    }
}