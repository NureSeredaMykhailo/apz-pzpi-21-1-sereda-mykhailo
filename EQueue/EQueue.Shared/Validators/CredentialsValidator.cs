using EQueue.Shared.Dto;
using FluentValidation;

namespace EQueue.Shared.Validators
{
    public class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator()
        {
            RuleFor(c => c.Login).NotEmpty().Length(4, 20);
            RuleFor(c => c.Password).NotEmpty().Length(8, 20);
        }
    }
}
