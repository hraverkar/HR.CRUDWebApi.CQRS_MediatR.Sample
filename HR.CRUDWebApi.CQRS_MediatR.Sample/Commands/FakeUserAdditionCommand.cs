using Bogus;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Commands
{
    public record FakeUserAdditionCommand : IRequest<List<ResponseDto>>;
    public class FakeUserAdditionCommandHandler(IRepository<User> repository, IMediator mediator, IEncryptionService encryptionService, IUnitOfWorks unitOfWorks) : IRequestHandler<FakeUserAdditionCommand, List<ResponseDto>>
    {
        private readonly IRepository<User> _repository = repository;
        private readonly IMediator _mediator = mediator;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly IUnitOfWorks _unitOfOfWorks = unitOfWorks;


        public async Task<List<ResponseDto>> Handle(FakeUserAdditionCommand request, CancellationToken cancellationToken)
        {
            var faker = new Faker();
            var userFaker = new Faker<User>()
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.Email, f => f.Person.FirstName + "_" + f.Person.LastName + "@gmail.com")
                .RuleFor(p => p.Password, f => _encryptionService.Encrypt(f.Internet.Password()))
                .RuleFor(p => p.Department, f => f.Random.String2(5));

            var users = userFaker.Generate(10);
            _repository.InsertAll(users);
            await _unitOfOfWorks.SaveChangesAsync(cancellationToken);

            // Convert users to ResponseDto
            var responseDtos = users.Select(u => new ResponseDto
            {
                Id = u.Id,
                ActionMessage = "User details saved successfully"
            }).ToList();

            return responseDtos;
        }
    }
}
