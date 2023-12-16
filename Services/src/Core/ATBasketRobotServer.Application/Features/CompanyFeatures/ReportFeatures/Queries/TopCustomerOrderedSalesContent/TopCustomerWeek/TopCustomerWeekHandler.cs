using ATBasketRobotServer.Application.Messaging;
using ATBasketRobotServer.Application.Services.CompanyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.TopCustomerOrderedSalesContent.TopCustomerWeek
{
    public sealed class TopCustomerWeekHandler : IQueryHandler<TopCustomerWeekQuey, TopCustomerWeekResponse>
    {
        private readonly IReportService _service;
        public TopCustomerWeekHandler(IReportService service)
        {
            _service = service;
        }

        public async Task<TopCustomerWeekResponse> Handle(TopCustomerWeekQuey request, CancellationToken cancellationToken)
        {

            var result = await _service.GetTopCustomerOrderedSalesContentWeekAsync(request.companyId, request.customerId);
            return new TopCustomerWeekResponse(result);
        }
    }
}
