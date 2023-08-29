namespace Smartwyre.DeveloperTest.Types;

// TODO: Future steps could remove the dependency on an enum so that the calculations
//  can be dynamically loaded with only a string name which wouldn't have to pre-exist.
public enum IncentiveType
{
    FixedRateRebate,
    AmountPerUom,
    FixedCashAmount
}
