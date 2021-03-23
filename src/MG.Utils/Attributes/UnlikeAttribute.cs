using System;
using System.ComponentModel.DataAnnotations;
using MG.Utils.I18N;

namespace MG.Utils.Attributes
{
    // copied from https://stackoverflow.com/a/35208420
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnlikeAttribute : ValidationAttribute
    {
        public string OtherProperty { get; private set; }

        public UnlikeAttribute(string otherProperty)
        {
            if (string.IsNullOrEmpty(otherProperty))
            {
                throw new ArgumentNullException(nameof(otherProperty));
            }

            OtherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string ErrorAccessor()
            {
                return string.Format(ErrorMessage ?? DataAnnotationErrorMessages.UnlikeError, validationContext.DisplayName, OtherProperty);
            }

            var otherProperty = validationContext.ObjectInstance.GetType()
                .GetProperty(OtherProperty);

            var otherPropertyValue = otherProperty!.GetValue(validationContext.ObjectInstance, null);

            return value.Equals(otherPropertyValue)
                ? new ValidationResult(ErrorAccessor())
                : ValidationResult.Success;
        }
    }
}