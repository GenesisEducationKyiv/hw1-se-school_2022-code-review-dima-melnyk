using FluentValidation;
using GSES.BusinessLogic.Validators;
using GSES.DataAccess.Entities;
using System.Threading.Tasks;
using Xunit;

namespace GSES.BusinessLogic.Tests.Validators
{
    public class SubscriberValidatorTests
    {
        [Fact]
        public void ValidateSubscriber_ThrowsValidationException()
        {
            // Arrange
            var validator = new SubscriberValidator();
            var invalidSubscriber = new Subscriber
            {
                Email = "notEmail"
            };

            // Act + Arrange
            Assert.ThrowsAsync<ValidationException>(() => validator.ValidateAndThrowAsync(invalidSubscriber));
        }

        [Fact]
        public async Task ValidateSubscriber_ValidatesSuccessfully()
        {
            // Arrange
            var validator = new SubscriberValidator();
            var validSubscriber = new Subscriber
            {
                Email = "email@email.com"
            };

            // Act
            var exception = await Record.ExceptionAsync(() => validator.ValidateAndThrowAsync(validSubscriber));

            // Assert
            Assert.Null(exception);
        }
    }
}
