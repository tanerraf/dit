using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Runner
{
    public class App
    {
        private readonly IRebateService _rebateService;

        public App(IRebateService rebateService)
        {
            _rebateService = rebateService;
        }

        public async Task<int> Run(string[] args)
        {
            var rebateOption = new Option<string>("--rebate")
            {
                Description = "The rebate identifier.",
                IsRequired = true
            };

            var productOption = new Option<string>("--product")
            {
                Description = "The product identifier.",
                IsRequired = true
            };

            var volumeOption = new Option<decimal>("--volume")
            {
                Description = "The product volume.",
                IsRequired = true
            };

            var rootCommand = new RootCommand("Command line runner app to calculate rebate for a given product and rebate");
            rootCommand.AddOption(rebateOption);
            rootCommand.AddOption(productOption);
            rootCommand.AddOption(volumeOption);

            rootCommand.SetHandler((value1, value2, value3) => CalculateRebateAsync(value1, value2, value3),
                rebateOption, productOption, volumeOption);

            return await rootCommand.InvokeAsync(args);

            async Task<int> CalculateRebateAsync(string rebate, string product, decimal volume)
            {
                var request = new CalculateRebateRequest
                {
                    RebateIdentifier = rebate,
                    ProductIdentifier = product,
                    Volume = volume
                };

                var result = _rebateService.Calculate(request);
                if (result.Success)
                {
                    Console.WriteLine($"Successfully calculated for {request}");
                }
                else
                {
                    Console.Error.WriteLine($"Unable to calculate for {request}");
                }

                return result.Success ? 0 : 1;
            }
        }
    }
}
