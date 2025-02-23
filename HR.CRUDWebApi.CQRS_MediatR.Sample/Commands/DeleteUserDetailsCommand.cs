using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Events;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Commands
{
    public record DeleteUserDetailsCommand(Guid UserID) : IRequest<ResponseDto>;
    public class DeleteUserDetailsCommandHandler(AppDbContext context, IMediator mediator) : IRequestHandler<DeleteUserDetailsCommand, ResponseDto>
    {
        private readonly AppDbContext _context = context;
        private readonly IMediator _mediator = mediator;

        public async Task<ResponseDto> Handle(DeleteUserDetailsCommand request, CancellationToken cancellationToken)
        {
            ResponseDto responseDto;
            try
            {
                if (request is not null)
                {
                    var user = await _context.Users.FindAsync(request.UserID);
                    if (user is not null)
                    {
                        _context.Users.Remove(user);
                        await _context.SaveChangesAsync();
                        responseDto = new ResponseDto(user.UserID, "User details deleted successfully");
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
                await _mediator.Publish(responseDto, cancellationToken);
                return responseDto;
            }
        }
    }
}
