using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Events
{
    public record ErrorEvent(ResponseDto Response) : INotification;
}
