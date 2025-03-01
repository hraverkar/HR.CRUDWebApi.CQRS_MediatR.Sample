using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Commands
{
    public record DeleteUserDetailsCommand(Guid UserID) : IRequest<ResponseDto>;
    public class DeleteUserDetailsCommandHandler(IMediator mediator, IRepository<User> repository, IUnitOfWorks unitOfWorks) : IRequestHandler<DeleteUserDetailsCommand, ResponseDto>
    {
        private readonly IRepository<User> _repository = repository;
        private readonly IMediator _mediator = mediator;
        private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;

        public async Task<ResponseDto> Handle(DeleteUserDetailsCommand request, CancellationToken cancellationToken)
        {
            ResponseDto responseDto;
            try
            {
                if (request is not null)
                {
                    var user = await _repository.GetByIdAsync(request.UserID);
                    if (user is not null)
                    {
                        _repository.Delete(user);
                        await _unitOfWorks.SaveChangesAsync(cancellationToken);
                        responseDto = new ResponseDto(user.Id, "User details deleted successfully");
                        await _mediator.Publish(new ResponseEvent(responseDto), cancellationToken);
                        return responseDto;
                    }

                    responseDto = new ResponseDto(Guid.Empty, "User not found");
                    await _mediator.Publish(new ResponseEvent(responseDto), cancellationToken);
                    return new ResponseDto(default, "User not found");
                }
                responseDto = new ResponseDto(Guid.Empty, "Invalid request");
                await _mediator.Publish(new ResponseEvent(responseDto), cancellationToken);
                return new ResponseDto(default, "Invalid request");

            }
            catch (Exception ex)
            {
                responseDto = new ResponseDto(default, ex.Message);
                await _mediator.Publish(new ResponseEvent(responseDto), cancellationToken);
                return responseDto;
            }
        }
    }
}
