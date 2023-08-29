using Smartwyre.DeveloperTest.Types;
using System.Collections.Generic;
using System.Linq;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore : IRebateDataStore
{
    private readonly HashSet<Rebate> _rebates;
    private readonly HashSet<RebateCalculation> _calculations;

    public RebateDataStore()
    {
        _rebates = GetRebates();
        _calculations = GetCalculations();

        HashSet<Rebate> GetRebates()
        {
            var json = System.IO.File.ReadAllText(@"rebates.json");
            return new(System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Rebate>>(json));
        }

        HashSet<RebateCalculation> GetCalculations()
        {
            if (System.IO.File.Exists(@"calculations.json"))
            {
                var json = System.IO.File.ReadAllText(@"calculations.json");
                return new(System.Text.Json.JsonSerializer.Deserialize<IEnumerable<RebateCalculation>>(json));
            }
            else
            {
                return new();
            }
        }
    }

    public Rebate? GetRebate(string rebateIdentifier)
    {
        return _rebates.FirstOrDefault(h => h.Identifier == rebateIdentifier);
    }

    public void StoreCalculationResult(Rebate rebate, decimal amount)
    {
        var id = _calculations.Count;
        RebateCalculation calculation = new()
        {
            Amount = amount,
            Id = id,
            Identifier = id.ToString(),
            IncentiveType = rebate.Incentive,
            RebateIdentifier = rebate.Identifier
        };

        _calculations.Add(calculation);

        var json = System.Text.Json.JsonSerializer.Serialize(_calculations);
        System.IO.File.WriteAllText(@"calculations.json", json);
    }
}
