using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("A entidade não pode ser vazia.")

                .NotNull()
                .WithMessage("A entidade não pode ser nula.");


            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O nome não pode ser nulo")

                .NotEmpty()
                .WithMessage("O nome não pode ser vazio")

                .MinimumLength(3)
                .WithMessage("O nome deve ter no mínimo 3 caracteres.")

                .MaximumLength(80)
                .WithMessage("O nome deve ter no máximo 80 caracteres.");


            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("O email não pode ser nulo")

                .MinimumLength(10)
                .WithMessage("O email deve ter no mínimo 10 caracteres")

                .MaximumLength(180)
                .WithMessage("O email deve ter no máximo 180 caracteres")

                .Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .WithMessage("O email informado não é válido.");


            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha não pode ser vazia")

                .NotNull()
                .WithMessage("A senha não pode ser nula")

                .MinimumLength(8)
                .WithMessage("A senha deve ter no mínimo 8 caracteres")

                .MaximumLength(80)
                .WithMessage("A senha deve ter no máximo 80 caracteres");
        }
    }
}
