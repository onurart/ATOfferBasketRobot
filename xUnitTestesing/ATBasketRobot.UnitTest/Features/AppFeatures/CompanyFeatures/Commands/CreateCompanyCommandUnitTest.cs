using ATBasketRobotServer.Domain.AppEntities;
using Azure.Core;
using Castle.Components.DictionaryAdapter;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Threading;
using ATBasketRobotServer.Application.Services.AppServices;
using Shouldly;
using Moq;
using ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Commands.CreateCompany;

namespace ATBasketRobot.UnitTest.Features.AppFeatures.CompanyFeatures.Commands
{
    public class CreateCompanyCommandUnitTest
    {
        private readonly Mock<ICompanyService> _companyService;
        private readonly CancellationToken _cancellationToken;
        public CreateCompanyCommandUnitTest(CancellationToken cancellationToken = default)
        {
            _companyService = new();
            _cancellationToken = cancellationToken;
        }

        [Fact]
        public async Task CompanyShoulBeNull()
        {
            Company company = (await _companyService.Object.GetCompanyByName("onur", _cancellationToken))!;
            company.ShouldBeNull();
        }

    }
}
