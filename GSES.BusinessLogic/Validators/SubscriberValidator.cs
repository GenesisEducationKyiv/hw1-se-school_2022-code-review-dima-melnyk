using FluentValidation;
using GSES.DataAccess.Entities;

namespace GSES.BusinessLogic.Validators
{
    public class SubscriberValidator : AbstractValidator<Subscriber>
    {
        public SubscriberValidator()
        {
            this.RuleFor(s => s.Email).EmailAddress();
        }
    }
}
