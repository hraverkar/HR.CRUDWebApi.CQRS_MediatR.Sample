using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Commands
{
    public record UpdateUserDetailsCommand(Guid UserID, string FirstName, string LastName, string Email, string Password, string Department) : IRequest<ResponseDto>;
    public class UpdateUserDetailsCommandHandler(IRepository<User> repository, IMediator mediator, IEncryptionService encryptionService, IUnitOfWorks unitOfWorks) : IRequestHandler<UpdateUserDetailsCommand, ResponseDto>
    {
        private readonly IRepository<User> _repository = repository;
        private readonly IMediator _mediator = mediator;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

        public async Task<ResponseDto> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            ResponseDto responseDto;
            try
            {
                if (request is not null)
                {

                    var user = _repository.GetByIdAsync(request.UserID).Result;
                    if (user is not null)
                    {
                        user.FirstName = request.FirstName;
                        user.LastName = request.LastName;
                        user.Email = request.Email;
                        user.Password = _encryptionService.Encrypt(request.Password);
                        user.Department = request.Department;
                        _repository.Update(user);
                        await _unitOfWorks.SaveChangesAsync(cancellationToken);
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
