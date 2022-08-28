using GSES.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GSES.API.Controllers
{
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRateService rateService;

        public RateController(IRateService rateService)
        {
            this.rateService = rateService;
        }

        [HttpGet("rate")]
        public async Task<IActionResult> Rate()
        {
            var rate = await this.rateService.GetRateAsync();

            return Ok(rate);
        }
    }
}
