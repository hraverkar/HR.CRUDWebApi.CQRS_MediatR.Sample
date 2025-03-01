using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events.Notifications;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Commands
{
    public record SaveUserDetailsCommand(string FirstName, string LastName, string Email, string Department, string Password) : IRequest<ResponseDto>;

    public class SaveUserDetailsCommandHandler(IRepository<User> repository, IMediator mediator, IEncryptionService encryptionService, IUnitOfWorks unitOfWorks) : IRequestHandler<SaveUserDetailsCommand, ResponseDto>
    {
        private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;
        private readonly IRepository<User> _repository = repository;
        private readonly IMediator _mediator = mediator;
        private readonly IEncryptionService _encryptionService = encryptionService;

        public async Task<ResponseDto> Handle(SaveUserDetailsCommand request, CancellationToken cancellationToken)
        {
            ResponseDto response;
            try
            {
                if (request is not null)
                {
                    var user = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Department = request.Department,
                        Password = _encryptionService.Encrypt(request.Password)
                    };
                    _repository.Insert(user);
                    await _unitOfWorks.SaveChangesAsync(cancellationToken);
                    response = new ResponseDto(user.Id, "User details saved successfully");
                    await _mediator.Publish(new ResponseEvent(response), cancellationToken);
                    await _mediator.Publish(new SaveUserDetailsNotification(response.Id, user.FirstName, user.Email), cancellationToken);
                    return new ResponseDto(user.Id, "User details saved successfully");
                }
                await _mediator.Publish(new ResponseEvent(new ResponseDto(Guid.Empty, "Invalid request")), cancellationToken);
                return new ResponseDto(Guid.Empty, "Invalid request");
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new ResponseEvent(new ResponseDto(Guid.Empty, ex.Message)), cancellationToken);
                return new ResponseDto(Guid.Empty, ex.Message);
            }
        }
    }
}
