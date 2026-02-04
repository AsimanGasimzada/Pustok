using FluentValidation;
using Pustok.Business.Helpers;

namespace Pustok.Business.Validators.ProductValidators;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().MaximumLength(256).MinimumLength(3).Must(x => x.Contains("a")).WithMessage("Adin daxilinde a simvolu olmalidir");


        RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(9999999);


        RuleFor(x => x.Image).Must(x => x?.CheckSize(2) ?? false).WithMessage("Seklin maksimum olcusu 2 mb olmalidir")
                            .Must(x => x?.CheckType("image") ?? false).WithMessage("Yalniz sekil formatinda data gondere bilersiniz");
    }
}


public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
{
    public ProductUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().MaximumLength(256).MinimumLength(3);
        RuleFor(x => x.Description).NotNull().MaximumLength(1024).MinimumLength(3);
        RuleFor(x => x.Price).NotNull().GreaterThanOrEqualTo(0).LessThanOrEqualTo(9999999);


        RuleFor(x => x.Image).Must(x => x?.CheckSize(2) ?? true).WithMessage("Seklin maksimum olcusu 2 mb olmalidir")
                          .Must(x => x?.CheckType("image") ?? true).WithMessage("Yalniz sekil formatinda data gondere bilersiniz");
    }

    //RuleForEach(x => x.TagIds).GreaterThan(0);

}
