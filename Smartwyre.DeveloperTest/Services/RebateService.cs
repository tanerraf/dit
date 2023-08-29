using Smartwyre.DeveloperTest.Calculations;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Linq;
using System.Reflection;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var result = new CalculateRebateResult();

        if (ValidateRequest(request))
        {
            var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
            if (rebate is null)
            {
                return result;
            }

            var product = _productDataStore.GetProduct(request.ProductIdentifier);
            if (product is null)
            {
                return result;
            }

            ICalculation? calc = GetCalculation(rebate.Incentive);

            if (calc is not null && calc.RebateValidForProduct(request, rebate, product))
            {
                var rebateAmount = calc.Calculate(request, rebate, product);

                result.Success = true;
                _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
            }
        }

        return result;

        ICalculation? GetCalculation(IncentiveType incentiveType)
        {
            var incentiveAttrType = typeof(IncentiveTypeAttribute);

            var iCalculationType = typeof(ICalculation);
            var selectedType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(c => iCalculationType.IsAssignableFrom(c) &&
                    c.GetCustomAttribute<IncentiveTypeAttribute>()?.IncentiveType == incentiveType);

            if (selectedType is not null)
            {
                return Activator.CreateInstance(selectedType) as ICalculation;
            }
            else
            {
                return null;
            }
        }
    }

    public bool ValidateRequest(CalculateRebateRequest request)
    {
        return !string.IsNullOrEmpty(request.RebateIdentifier) &&
           !string.IsNullOrEmpty(request.ProductIdentifier) &&
           request.Volume > 0;
    }

}
