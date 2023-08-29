using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Calculations
{
    public interface ICalculation
    {
        decimal Calculate(CalculateRebateRequest request, Rebate rebate, Product product);
        bool RebateValidForProduct(CalculateRebateRequest request, Rebate rebate, Product product);
    }
}
