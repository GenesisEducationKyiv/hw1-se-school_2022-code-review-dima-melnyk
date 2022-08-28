using FluentValidation;
using GSES.DataAccess.Entities;

namespace GSES.BusinessLogic.Validators
{
    public class SubscriberValidator : AbstractValidator<Subscriber>
    {
        public SubscriberValidator()
        {
            RuleFor(s => s.Email).EmailAddress();
        }
    }
}
