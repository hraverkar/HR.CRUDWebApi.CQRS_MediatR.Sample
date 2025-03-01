using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Queries
{
    public record GetUserListQuery() : IRequest<List<UserInfo>>;

    public class GetUserListQueryHandler(IEncryptionService encryptionService, IRepository<User> repository) : IRequestHandler<GetUserListQuery, List<UserInfo>>
    {
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IRepository<User> _repository = repository;

        async Task<List<UserInfo>> IRequestHandler<GetUserListQuery, List<UserInfo>>.Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var allUser = await _repository.GetAll().Where(i => !i.IsDeleted).ToListAsync(cancellationToken);
            var userList = allUser.Select(user =>
            {
                user.Password = _encryptionService.Decrypt(user.Password);
                return new UserDto
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
                };
            }).ToList();

            var userInfo = new UserInfo
            {
                Count = userList.Count,
                UserDto = userList
            };
            return [userInfo];

        }
    }
}
