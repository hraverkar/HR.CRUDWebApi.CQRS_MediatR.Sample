using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Queries
{
    public record GetUserListQuery() : IRequest<List<UserDto>>;
    
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<UserDto>>
    {
        private readonly AppDbContext _context;
        private readonly IEncryptionService _encryptionService;
        public GetUserListQueryHandler(AppDbContext context, IEncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
        }
        public async Task<List<UserDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users.Select(u => new UserDto
            {
                UserID = u.UserID,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Department = u.Department,
                Password = _encryptionService.Decrypt(u.Password)
            }).ToListAsync();
        }
    }
}
