using GSES.BusinessLogic.Models.Rate;
using GSES.BusinessLogic.Processors.Interfaces;
using System.Threading.Tasks;

namespace GSES.BusinessLogic.Processors
{
    public abstract class BaseRateProcessor : IProcessorChain, IRateProcessor
    {
        public IProcessorChain Next { get; set; }

        public async Task<BaseRateModel> Handle()
        {
            var result = await this.GetRateAsync();

            if (result.Item2)
            {
                return result.Item1;
            }
            else if (this.Next is null)
            {
                return null;
            }

            return await this.Next.Handle();
        }

        public abstract Task<(BaseRateModel, bool)> GetRateAsync();
    }
}
