using Displasrios.Recaudacion.Core.Models;
using FluentValidation;

namespace Displasrios.Recaudacion.Infraestructure.Validations
{
    public class UserCreationValidator : AbstractValidator<UserCreation>
    {
        public UserCreationValidator()
        {
            RuleFor(x => x.Identification).NotNull().NotEmpty().WithMessage("Se esperaba la {PropertyName}.").MinimumLength(10).WithMessage("{PropertyName} debe tener mínimo 10 dígitos.");
            RuleFor(x => x.Names).NotNull().NotEmpty().MinimumLength(3).WithMessage("Se esperaba la {PropertyName}. Mínimo 3 dígitos.");
            RuleFor(x => x.Surnames).NotNull().NotEmpty().MinimumLength(3).WithMessage("Se esperaba la {PropertyName}. Mínimo 3 dígitos.");
            RuleFor(x => x.Type).GreaterThan(0).WithMessage("{PropertyName} debe ser mayor a cero.");
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("{PropertyName} es obligatorio").EmailAddress().WithMessage("{PropertyName} debe ser una dirección válida.");
        }
    }

    public class CustomerCreationValidator : AbstractValidator<CustomerCreate>
    {
        public CustomerCreationValidator()
        {
            RuleFor(x => x.Identification).NotNull().NotEmpty().WithMessage("Se esperaba la {PropertyName}.").MinimumLength(10).WithMessage("{PropertyName} debe tener mínimo 10 dígitos.");
            RuleFor(x => x.Names).NotNull().NotEmpty().MinimumLength(3).WithMessage("Se esperaba {PropertyName}. Mínimo 3 dígitos.");
            RuleFor(x => x.Surnames).NotNull().NotEmpty().MinimumLength(3).WithMessage("Se esperaba {PropertyName}. Mínimo 3 dígitos.");
            RuleFor(x => x.Address).NotNull().NotEmpty().MinimumLength(3).WithMessage("{PropertyName} debe ser mayor a cero.");
            RuleFor(x => x.Phone).NotNull().NotEmpty().MinimumLength(10).MaximumLength(10).WithMessage("{PropertyName} debe ser de 10 dígitos.");
            RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("{PropertyName} es obligatorio").EmailAddress().WithMessage("{PropertyName} debe ser una dirección válida.");
        }
    }
}
