using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using MG.Utils.AspNetCore.Validation;
using MG.Utils.I18N;
using Microsoft.AspNetCore.Http;

namespace MG.Utils.AspNetCore.Attributes
{
    // copied from https://stackoverflow.com/questions/56588900/how-to-validate-uploaded-file-in-asp-net-core
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly IReadOnlyCollection<string> _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);

                if (extension == null)
                {
                    return new ValidationResult(validationContext.ErrorMessageWithDisplayName(
                        DataAnnotationErrorMessages.FileExtensionIsRequired));
                }

                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(validationContext.ErrorMessageWithDisplayName(
                        DataAnnotationErrorMessages.FileExtensionIsNotAllowed));
                }

                return ValidationResult.Success;
            }

            throw new InvalidOperationException("You should use this attribute only for File properties");
        }
    }
}