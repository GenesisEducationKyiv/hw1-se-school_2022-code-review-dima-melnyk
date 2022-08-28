using GSES.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GSES.API.Controllers
{
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriberService subscriberService;

        public SubscriptionController(ISubscriberService subscriberService)
        {
            this.subscriberService = subscriberService;
        }

        [HttpPost("subscribe")]
        public Task Subscribe(string email) => this.subscriberService.AddSubscriberAsync(email);

        [HttpPost("sendEmails")]
        public Task SendEmails() => this.subscriberService.SendRateForAllTheSubscribers();
    }
}
