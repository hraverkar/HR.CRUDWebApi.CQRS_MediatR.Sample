using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events.Notifications;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Commands
{
    public record SaveUserDetailsCommand(string FirstName, string LastName, string Email, string Department, string Password) : IRequest<ResponseDto>;

    public class SaveUserDetailsCommandHandler(AppDbContext context, IMediator mediator, IEncryptionService encryptionService) : IRequestHandler<SaveUserDetailsCommand, ResponseDto>
    {
        private readonly AppDbContext _context = context;
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
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    response = new ResponseDto(user.Id, "User details saved successfully");
                    await _mediator.Publish(new ResponseEvent(response));
                    await _mediator.Publish(new SaveUserDetailsNotification(response.Id, user.FirstName, user.Email));
                    return new ResponseDto(user.Id, "User details saved successfully");
                }
                await _mediator.Publish(new ResponseEvent(new ResponseDto(Guid.Empty, "Invalid request")));
                return new ResponseDto(Guid.Empty, "Invalid request");
            }
            catch (Exception ex)
            {
                await _mediator.Publish(new ResponseEvent(new ResponseDto(Guid.Empty, ex.Message)));
                return new ResponseDto(Guid.Empty, ex.Message);
            }
        }
    }
}
