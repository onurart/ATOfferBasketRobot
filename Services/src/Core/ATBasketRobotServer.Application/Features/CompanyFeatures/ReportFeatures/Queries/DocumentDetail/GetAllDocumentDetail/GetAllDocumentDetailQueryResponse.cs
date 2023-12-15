using ATBasketRobotServer.Domain.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.DocumentDetail.GetAllDocumentDetail;
public sealed record GetAllDocumentDetailQueryResponse(IList<DocumentDetailDto> data);