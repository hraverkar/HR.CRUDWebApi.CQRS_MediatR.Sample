using FluentValidation;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Commands;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Validations
{
    public class SaveUserDetailsValidation : AbstractValidator<SaveUserDetailsCommand>
    {
        public SaveUserDetailsValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Department).NotEmpty().WithMessage("Department is required");
        }
    }
}
