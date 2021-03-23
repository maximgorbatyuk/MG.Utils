using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using MG.Utils.I18N;

namespace MG.Utils.Attributes
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
            return value switch
            {
                null => new ValidationResult(ErrorMessage ?? DataAnnotationErrorMessages.CollectionNotEmpty),
                IEnumerable enumerable => enumerable.GetEnumerator().MoveNext()
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage ?? DataAnnotationErrorMessages.CollectionNotEmpty),
                _ => throw new InvalidOperationException("Do not use this attribute for non-collection properties")
            };
        }
    }
}