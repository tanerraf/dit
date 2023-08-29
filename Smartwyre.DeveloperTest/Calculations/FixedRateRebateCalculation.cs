using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculations
{
    [IncentiveType(IncentiveType.FixedRateRebate)]
    public class FixedRateRebateCalculation : ICalculation
    {
        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return product.Price * rebate.Percentage * request.Volume;
        }

        public bool RebateValidForProduct(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return product.Price > 0 && rebate.Percentage > 0 && request.Volume > 0 &&
                product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) &&
                rebate.Incentive is IncentiveType.FixedRateRebate;
        }
    }
}
