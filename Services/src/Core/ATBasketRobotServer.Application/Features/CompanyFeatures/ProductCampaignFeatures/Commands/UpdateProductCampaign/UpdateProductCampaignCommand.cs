using ATBasketRobotServer.Application.Messaging;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.UpdateProductCampaign;
public sealed record UpdateProductCampaignCommand(string Id, int? ProductReferance, string? ProductCode, string? ProductGroup, double? MinOrder, string companyId) :ICommand<UpdateProductCampaignCommandResponse>;