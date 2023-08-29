using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculations
{
    [IncentiveType(IncentiveType.FixedCashAmount)]
    public class FixedCashAmountCalculation : ICalculation
    {
        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return rebate.Amount;
        }

        public bool RebateValidForProduct(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return rebate.Amount > 0 &&
                product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount) &&
                rebate.Incentive is IncentiveType.FixedCashAmount;
        }
    }
}
