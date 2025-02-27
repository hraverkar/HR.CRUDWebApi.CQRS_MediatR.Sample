using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Queries
{
    public record GetUserListQuery() : IRequest<List<UserDto>>;

    public class GetUserListQueryHandler(IEncryptionService encryptionService, IRepository<User> repository) : IRequestHandler<GetUserListQuery, List<UserDto>>
    {
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IRepository<User> _repository = repository;

        async Task<List<UserDto>> IRequestHandler<GetUserListQuery, List<UserDto>>.Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var allUser = _repository.GetAll();
            var userList = allUser.ToList();
            userList.ForEach(user =>
            {
                user.Password = _encryptionService.Decrypt(user.Password);
            });
            return await Task.FromResult(userList.Select(user => new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Department = user.Department,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                IsDeleted = user.IsDeleted
            }).ToList());
        }
    }
}
