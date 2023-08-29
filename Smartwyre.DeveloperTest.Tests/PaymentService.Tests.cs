using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{
    private readonly Mock<IRebateDataStore> _rebateDataStore;
    private readonly Mock<IProductDataStore> _productDataStore;
    private readonly IRebateService _rebateService;

    public PaymentServiceTests()
    {
        _rebateDataStore = new Mock<IRebateDataStore>();
        _rebateDataStore.Setup(repo => repo.GetRebate("Reb-FRR"))
            .Returns(new Rebate()
            {
                Identifier = "Reb-FRR",
                Incentive = IncentiveType.FixedRateRebate,
                Percentage = 15
            });
        _rebateDataStore.Setup(repo => repo.GetRebate("Reb-FCA"))
            .Returns(new Rebate()
            {
                Identifier = "Reb-FCA",
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 5
            });
        _rebateDataStore.Setup(repo => repo.GetRebate("Reb-APU"))
            .Returns(new Rebate()
            {
                Identifier = "Reb-APU",
                Incentive = IncentiveType.AmountPerUom,
                Amount = 0.25m
            });

        _productDataStore = new Mock<IProductDataStore>();
        _productDataStore.Setup(repo => repo.GetProduct("Prod-A"))
            .Returns(new Product()
            {
                Identifier = "Prod-A",
                Price = 4200,
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.FixedCashAmount
            });
        _productDataStore.Setup(repo => repo.GetProduct("Prod-Z"))
            .Returns(new Product()
            {
                Identifier = "Prod-Z",
                Price = 100,
                SupportedIncentives = SupportedIncentiveType.AmountPerUom
            });

        _rebateService = new RebateService(_rebateDataStore.Object, _productDataStore.Object);
    }

    [Theory]
    [InlineData("Reb-FRR", "Prod-Z", 1)]
    [InlineData("Reb-FCA", "Prod-A", 100)]
    [InlineData("Reb-APU", "Prod-A", 1000)]
    public void ValidateRequest_AllInputsSet_ReturnTrue(string rebate, string product, decimal volume)
    {
        // Arrange.
        var request = new CalculateRebateRequest() { ProductIdentifier = product, RebateIdentifier = rebate, Volume = volume };

        // Act.
        var result = _rebateService.ValidateRequest(request);

        // Assert.
        Assert.True(result);
    }

    [Theory]
    [InlineData("", "Prod-Z", 1)]
    [InlineData("Reb-FCA", "", 100)]
    [InlineData("Reb-FRR", "Prod-Z", 0)]
    public void ValidateRequest_MissingInputs_ReturnFalse(string rebate, string product, decimal volume)
    {
        // Arrange.
        var request = new CalculateRebateRequest() { ProductIdentifier = product, RebateIdentifier = rebate, Volume = volume };

        // Act.
        var result = _rebateService.ValidateRequest(request);

        // Assert.
        Assert.False(result);
    }

    [Theory]
    [InlineData("Reb-FRR", "Prod-A", 1)]
    [InlineData("Reb-FCA", "Prod-A", 100)]
    [InlineData("Reb-APU", "Prod-Z", 100)]
    public void Calculate_RebateProductCombinationSupported_ReturnTrue(string rebate, string product, decimal volume)
    {
        // Arrange.
        var request = new CalculateRebateRequest() { ProductIdentifier = product, RebateIdentifier = rebate, Volume = volume };

        // Act.
        var result = _rebateService.Calculate(request);

        // Assert.
        Assert.True(result.Success);
    }

    [Theory]
    [InlineData("", "Prod-Z", 1)]
    [InlineData("Reb-FCA", "", 100)]
    [InlineData("Reb-FRR", "Prod-Z", 0)]
    [InlineData("Reb-FRR", "Prod-Z", 1)]
    [InlineData("Reb-FCA", "Prod-Z", 100)]
    [InlineData("Reb-APU", "Prod-A", 100)]
    public void Calculate_RebateProductCombinationSupported_ReturnFalse(string rebate, string product, decimal volume)
    {
        // Arrange.
        var request = new CalculateRebateRequest() { ProductIdentifier = product, RebateIdentifier = rebate, Volume = volume };

        // Act.
        var result = _rebateService.Calculate(request);

        // Assert.
        Assert.False(result.Success);
    }
}
