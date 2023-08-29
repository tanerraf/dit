using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculations
{
    [IncentiveType(IncentiveType.AmountPerUom)]
    public class AmountPerUomCalculation : ICalculation
    {
        public decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return rebate.Amount * request.Volume;
        }

        public bool RebateValidForProduct(CalculateRebateRequest request, Rebate rebate, Product product)
        {
            return rebate.Amount > 0 && request.Volume > 0 &&
                product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) &&
                rebate.Incentive is IncentiveType.AmountPerUom;
        }
    }
}
