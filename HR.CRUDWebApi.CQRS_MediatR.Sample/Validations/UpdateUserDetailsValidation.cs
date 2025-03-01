using FluentValidation;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Commands;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Validations
{
    public class UpdateUserDetailsValidation :  AbstractValidator<UpdateUserDetailsCommand>
    {
        public UpdateUserDetailsValidation()
        {
            RuleFor(x => x.UserID).NotEmpty().WithMessage("User ID is required")
                .Must(ValidateGuid).WithMessage("Invalid User ID");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required")
                .MaximumLength(3).WithMessage("The First Name field's max length is 3");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Department).NotEmpty().WithMessage("Department is required");
        }

        private bool ValidateGuid(Guid guid)
        {
            return Guid.TryParse(guid.ToString(), out _);
        }
    }
}
