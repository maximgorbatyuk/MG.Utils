using System.ComponentModel.DataAnnotations;
using MG.Utils.Exceptions;
using MG.Utils.I18N;
using MG.Utils.Validators;
using Xunit;

namespace MG.Utils.Test
{
    public class EntityValidatorTest
    {
        [Fact]
        public void Valid_EntityIsValid_Ok()
        {
            var target = new EntityValidator<Entity>(new Entity
            {
                RequiredTitle = "Awesome",
                NonRequiredTitle = null,
                Year = 2020
            });

            Assert.True(target.Valid());

            target.ThrowIfInvalid();
        }

        [Fact]
        public void Valid_EntityIsInvalid_Ok()
        {
            var target = new EntityValidator<Entity>(new Entity
            {
                RequiredTitle = "Awesome title 1234567890",
                NonRequiredTitle = "Awesome nit-required title 1234567890",
                Year = 3020
            });

            Assert.False(target.Valid());
        }

        [Fact]
        public void ThrowIfInvalid_EntityIsInvalid_Exception()
        {
            var target = new EntityValidator<Entity>(new Entity
            {
                RequiredTitle = "Awesome title 1234567890",
                NonRequiredTitle = "Awesome nit-required title 1234567890",
                Year = 3020
            });

            Assert.Throws<EntityInvalidException>(() => target.ThrowIfInvalid());
        }

        public class Entity
        {
            [Required(ErrorMessage = DataAnnotationErrorMessages.Required)]
            [StringLength(10, ErrorMessage = DataAnnotationErrorMessages.StringLength)]
            public string RequiredTitle { get; set; }

            [StringLength(15, ErrorMessage = DataAnnotationErrorMessages.StringLength)]
            public string NonRequiredTitle { get; set; }

            [Range(1900, 3000)]
            public int Year { get; set; }
        }
    }
}