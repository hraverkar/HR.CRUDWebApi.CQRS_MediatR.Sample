using HR.CRUDWebApi.CQRS_MediatR.Sample.Context;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;
        private bool disposedValue;

        public UnitOfWorks(AppDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
