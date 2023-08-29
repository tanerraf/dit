namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateRequest
{
    public string RebateIdentifier { get; set; }

    public string ProductIdentifier { get; set; }

    public decimal Volume { get; set; }

    public override string ToString()
    {
        return $"RebateIdentifier: '{RebateIdentifier}', ProductIdentifier: '{ProductIdentifier}', Volume: '{Volume:0.####}'";
    }
}
