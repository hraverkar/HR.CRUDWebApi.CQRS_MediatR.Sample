using Bogus;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Commands
{
    public record FakeUserAdditionCommand : IRequest<List<ResponseDto>>;
    public class FakeUserAdditionCommandHandler(AppDbContext appDbContext, IMediator mediator, IEncryptionService encryptionService) : IRequestHandler<FakeUserAdditionCommand, List<ResponseDto>>
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly IMediator _mediator = mediator;
        private readonly IEncryptionService _encryptionService = encryptionService;


        public async Task<List<ResponseDto>> Handle(FakeUserAdditionCommand request, CancellationToken cancellationToken)
        {
            var faker = new Faker();
            var userFaker = new Faker<User>()
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.Email, f => f.Person.FirstName + "_" + f.Person.LastName + "@gmail.com")
                .RuleFor(p => p.Password, f => f.Internet.Password())
                .RuleFor(p => p.Department, f => f.Random.String2(5));

            var users = userFaker.Generate(1000);
            _appDbContext.Users.AddRange(users);
            await _appDbContext.SaveChangesAsync();

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
