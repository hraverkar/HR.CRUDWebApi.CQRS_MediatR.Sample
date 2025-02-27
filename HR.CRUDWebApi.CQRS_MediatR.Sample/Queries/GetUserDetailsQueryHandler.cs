using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Queries
{
    public record GetUserDetailsByUserIDQuery(Guid UserID) : IRequest<UserDto>;
    public class GetUserDetailsQueryHandler(IRepository<User> repository, IUnitOfWorks unitOfWorks, IEncryptionService encryptionService) : IRequestHandler<GetUserDetailsByUserIDQuery, UserDto>
    {
        private readonly IRepository<User> _repository = repository;
        private readonly IUnitOfWorks _unitOfWorks = unitOfWorks;
        private readonly IEncryptionService _encryptionService = encryptionService;

        async Task<UserDto> IRequestHandler<GetUserDetailsByUserIDQuery, UserDto>.Handle(GetUserDetailsByUserIDQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.UserID);
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Department = user.Department,
                Password = _encryptionService.Decrypt(user.Password),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsDeleted = user.IsDeleted
            };
        }
    }
}
