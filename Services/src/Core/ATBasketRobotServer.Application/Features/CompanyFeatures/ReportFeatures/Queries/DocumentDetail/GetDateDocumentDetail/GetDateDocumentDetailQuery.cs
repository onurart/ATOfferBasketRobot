using ATBasketRobotServer.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetDateDocumentDetail;
public sealed record GetDateDocumentDetailQuery(string companyId,string date) : IQuery<GetDateDocumentDetailQueryResponse>;