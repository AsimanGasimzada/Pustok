using FluentValidation;
using Pustok.Business.Dtos.UserDtos;

namespace Pustok.Business.Validators.UserValidators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull().NotEmpty()
            .EmailAddress().WithMessage("Email duzgun formatda olmalidir")
            .MaximumLength(256);

        RuleFor(x => x.Fullname)
            .NotNull().NotEmpty()
            .MinimumLength(3).WithMessage("Ad ve soyad en az 3 simvoldan ibaret olmalidir")
            .MaximumLength(256);

        RuleFor(x => x.UserName)
            .NotNull().NotEmpty()
            .MinimumLength(3).WithMessage("Istifadeci adi en az 3 simvoldan ibaret olmalidir")
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotNull().NotEmpty()
            .MinimumLength(6).WithMessage("Parol en az 6 simvol olmalidir")
            .MaximumLength(100);
        //.Matches("[^a-zA-Z0-9]").WithMessage("Parol en az 1 xüsusi simvol (misal: !@#$) olmalidir")
        //.Matches("[A-Z]").WithMessage("Parol en az 1 boyuk herf olmalidir")
        //.Matches("[a-z]").WithMessage("Parol en az 1 kicik herf olmalidir")
        //.Matches("[0-9]").WithMessage("Parol en az 1 reqem olmalidir")

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Parollar uyqunlasmadi");
    }
}
