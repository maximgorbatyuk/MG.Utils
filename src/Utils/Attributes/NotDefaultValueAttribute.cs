using System;
using System.ComponentModel.DataAnnotations;
using Utils.I18N;

namespace Utils.Attributes
{
    /// <summary>
    /// Checks that the property contains not-default value. Returns true if the property is not value type of.
    /// </summary>
    public class NotDefaultValueAttribute : ValidationAttribute
    {
        // Copied from https://andrewlock.net/creating-an-empty-guid-validation-attribute/
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // NotDefaultValue doesn't necessarily mean required
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string ErrorAccessor()
            {
                return validationContext.ErrorMessageWithDisplayName(ErrorMessage ?? DataAnnotationErrorMessages.NotDefaultValue);
            }

            var type = value.GetType();
            switch (type.IsValueType)
            {
                case true:
                {
                    var defaultValue = Activator.CreateInstance(type);
                    return !value.Equals(defaultValue)
                        ? ValidationResult.Success
                        : new ValidationResult(ErrorAccessor());
                }

                default:
                    // non-null ref type
                    return ValidationResult.Success;
            }
        }
    }
}