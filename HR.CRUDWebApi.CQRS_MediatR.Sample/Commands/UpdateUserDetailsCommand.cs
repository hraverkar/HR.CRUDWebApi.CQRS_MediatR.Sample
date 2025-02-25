using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Commands
{
    public record UpdateUserDetailsCommand(Guid UserID, string FirstName, string LastName, string Email, string Password, string Department) : IRequest<ResponseDto>;
    public class UpdateUserDetailsCommandHandler(AppDbContext appDbContext, IMediator mediator, IEncryptionService encryptionService) : IRequestHandler<UpdateUserDetailsCommand, ResponseDto>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly IMediator _mediator = mediator;
        private readonly IEncryptionService _encryptionService = encryptionService;

        public async Task<ResponseDto> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            ResponseDto responseDto;
            try
            {
                if (request is not null)
                {
                    var user = _appDbContext.Users.Find(request.UserID);
                    if (user is not null)
                    {
                        user.FirstName = request.FirstName;
                        user.LastName = request.LastName;
                        user.Email = request.Email;
                        user.Password = _encryptionService.Encrypt(request.Password);
                        user.Department = request.Department;
                        _appDbContext.Users.Update(user);
                        _appDbContext.SaveChanges();
                        responseDto = new ResponseDto(user.Id, "User details updated successfully");
                        await _mediator.Publish(new ResponseEvent(responseDto));
                        return responseDto;
                    }
                    else
                    {
                        responseDto = new ResponseDto(default, "User not found");
                        await _mediator.Publish(new ResponseEvent(responseDto));
                        return responseDto;
                    }
                }
                else
                {
                    responseDto = new ResponseDto(default, "Invalid request");
                    await _mediator.Publish(new ResponseEvent(responseDto));
                    return responseDto;
                }
            }
            catch (Exception ex)
            {
                responseDto = new ResponseDto(default, ex.Message);
                await _mediator.Publish(new ResponseEvent(responseDto));
                return responseDto;
            }
        }
    }
}
