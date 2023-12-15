using ATBasketRobotServer.Application.Features.AppFeatures.CompanyFeatures.Commands.CreateCompany;
using ATBasketRobotServer.Application.Features.AppFeatures.RoleFeatures.Commands.CreateRole;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatus;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Commands.CreateBasketStatusCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.BasketStatusFeatures.Queries.GetAllBasketStatusDto;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.CustomerFeatures.Commands.CreateCustomerCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocument;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Commands.CreateDocumentCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.DocumentFeatures.Queries.GetDocOffCustTotal;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOffer;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Commands.CreateOfferCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.OfferFeatures.Queries.GetAllOfferDto;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaign;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductCampaignFeatures.Commands.CreateProductCampaignCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProduct;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductAll;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.CreateProductCompany;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerMonth;
using ATBasketRobotServer.Application.Features.CompanyFeatures.ReportFeatures.Queries.OfferedOrderCustomer.GetOfferedOrderCustomerYear;
using ATBasketRobotServer.Domain.AppEntities;
using ATBasketRobotServer.Domain.AppEntities.Identity;
using ATBasketRobotServer.Domain.CompanyEntities;
using ATBasketRobotServer.Domain.Dtos;
using ATBasketRobotServer.Domain.Dtos.Report;
using AutoMapper;
namespace ATBasketRobotServer.Persistance.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCompanyCommand, Company>();
        CreateMap<CreateRoleCommand, AppRole>();

        CreateMap<CreateProductCommand, Product>();
        CreateMap<CreateProductAllCommand, List<Product>>();
        CreateMap<CreateProductCompanyCommand, List<Product>>();

        CreateMap<CreateBasketStatusCommand, BasketStatus>();
        CreateMap<CreateBasketStatusAllCommand, List<BasketStatus>>();
        CreateMap<CreateBasketStatusCompanyCommand, List<BasketStatus>>();

        CreateMap<GetAllBasketStatusDtoQuery, List<BasketStatusDto>>();

        //CreateMap<BasketStatus,BasketStatusDto>().ReverseMap();
        //CreateMap<List<BasketStatus>,List<BasketStatusDto>>();

        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<CreateCustomerAllCommand, List<Customer>>();
        CreateMap<CreateCustomerCompanyCommand, List<Customer>>();

        CreateMap<CreateDocumentCommand, Document>();
        CreateMap<CreateDocumentAllCommand, List<Document>>();
        CreateMap<CreateDocumentCompanyCommand, List<Document>>();

        CreateMap<CreateProductCampaignCommand, ProductCampaign>();
        CreateMap<CreateProductCampaignAllCommand, List<ProductCampaign>>();
        CreateMap<CreateProductCampaignCompanyCommand, List<ProductCampaign>>();

        CreateMap<CreateOfferCommand, Offer>();
        CreateMap<CreateOfferAllCommand, List<Offer>>();
        CreateMap<CreateOfferCompanyCommand, List<Offer>>();
        CreateMap<GetDocOffCustTotalQuery, List<DocOffCustTotalDto>>();

        CreateMap<GetAllOfferDtoQuery, List<OfferDto>>();

        CreateMap<GetOfferedOrderYearQuery, List<OrderedCustomerYearDto>>();
        CreateMap<GetOfferedOrderMonthQuery, List<OrderedCustomerMonthDto>>();
    }
}