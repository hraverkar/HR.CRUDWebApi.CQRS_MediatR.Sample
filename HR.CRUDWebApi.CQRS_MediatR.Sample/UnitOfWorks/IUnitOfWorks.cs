using HR.CRUDWebApi.CQRS_MediatR.Sample.Repositories.Interfaces;
using System;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.UnitOfWorks
{
    public interface IUnitOfWorks : IDisposable
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
        IRepository<T> GetRepository<T>() where T : class;
    }
}
