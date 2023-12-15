using FluentValidation;
namespace ATBasketRobotServer.Application.Features.CompanyFeatures.ProductFeatures.Commands.RemoveByIdProduct;
public sealed class RemoveByIdProductCommandValidator : AbstractValidator<RemoveByIdProductCommand>
{
    public RemoveByIdProductCommandValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("Id alanı boş olamaz!");
        RuleFor(p => p.Id).NotNull().WithMessage("Id alanı boş olamaz!");
        RuleFor(p => p.companyId).NotNull().WithMessage("Şirket bilgisi boş olamaz!");
        RuleFor(p => p.companyId).NotEmpty().WithMessage("Şirket bilgisi boş olamaz!");
    }
}