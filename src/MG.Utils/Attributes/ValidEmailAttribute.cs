using System;
using System.ComponentModel.DataAnnotations;
using MG.Utils.Helpers;
using MG.Utils.I18N;

namespace MG.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ValidEmailAttribute : ValidationAttribute
    {
        private static readonly EmailValidRegex _emailRegex = new ();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // ValidEmailAttribute doesn't necessarily mean required
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not string valueString)
            {
                return new ValidationResult(ErrorMessage ?? DataAnnotationErrorMessages.InvalidEmail);
            }

            return _emailRegex.IsValid(valueString)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? DataAnnotationErrorMessages.InvalidEmail);
        }
    }
}