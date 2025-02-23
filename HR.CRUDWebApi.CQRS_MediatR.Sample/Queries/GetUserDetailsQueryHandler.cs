using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Queries
{
    public record GetUserDetailsByUserIDQuery(Guid UserID) : IRequest<UserDto>;
    public class GetUserDetailsQueryHandler(AppDbContext context, IEncryptionService encryptionService) : IRequestHandler<GetUserDetailsByUserIDQuery, UserDto>
    {
        private readonly AppDbContext _context = context;
        private readonly IEncryptionService _encryptionService = encryptionService;

        public async Task<UserDto> Handle(GetUserDetailsByUserIDQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(request.UserID);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            return new UserDto
            {
                UserID = user.UserID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Department = user.Department,
                Password = _encryptionService.Decrypt(user.Password)
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
