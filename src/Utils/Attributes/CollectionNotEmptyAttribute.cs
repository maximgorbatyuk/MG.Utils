using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Utils.I18N;

namespace Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CollectionNotEmptyAttribute : ValidationAttribute
    {
        public CollectionNotEmptyAttribute(string errorMessage = null)
            : base(errorMessage ?? DataAnnotationErrorMessages.CollectionNotEmpty)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string ErrorAccessor()
            {
                return validationContext.ErrorMessageWithDisplayName(
                    ErrorMessage ?? DataAnnotationErrorMessages.CollectionNotEmpty);
            }

            return value switch
            {
                null => new ValidationResult(ErrorAccessor()),
                IEnumerable enumerable => enumerable.GetEnumerator().MoveNext()
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorAccessor()),
                _ => throw new InvalidOperationException("Do not use this attribute for non-collection properties")
            };
        }
    }
}