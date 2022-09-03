using FluentValidation;
using GSES.BusinessLogic.Consts;
using GSES.BusinessLogic.Extensions;
using GSES.BusinessLogic.Processors.Interfaces;
using GSES.BusinessLogic.Services.Interfaces;
using GSES.DataAccess.Entities;
using GSES.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository subscriberRepository;
        private readonly SmtpClient smtpClient;
        private readonly IRateProcessor rateProcessor;
        private readonly IValidator<Subscriber> validator;

        private readonly string senderEmail;

        public SubscriberService(
            ISubscriberRepository subscriberRepository,
            SmtpClient smtpClient,
            IValidator<Subscriber> validator,
            IRateProcessor rateProcessor,
            IConfiguration configuration)
        {
            this.subscriberRepository = subscriberRepository;
            this.smtpClient = smtpClient;
            this.validator = validator;
            this.rateProcessor = rateProcessor;

            this.senderEmail = configuration[EmailConsts.SmtpEmail];
        }

        public async Task AddSubscriberAsync(string email)
        {
            var subscriber = new Subscriber
            {
                Email = email,
            };

            await this.validator.ValidateAndThrowAsync(subscriber);
            await this.subscriberRepository.AddAsync(subscriber);
        }

        public async Task SendRateForAllTheSubscribers()
        {
            var subscribers = await this.subscriberRepository.GetAsync();
            var emails = subscribers.Select(s => s.Email);
            var rate = await this.rateProcessor.GetRateAsync();

            var subject = string.Format(EmailConsts.EmailSubject, RateConsts.BitcoinCode, RateConsts.HryvnyaCode);
            var body = string.Format(EmailConsts.EmailBody, RateConsts.BitcoinCode, RateConsts.HryvnyaCode, rate.Rate);

            await this.smtpClient.SendMessagesOnEmails(this.senderEmail, emails, subject, body);
        }
    }
}
