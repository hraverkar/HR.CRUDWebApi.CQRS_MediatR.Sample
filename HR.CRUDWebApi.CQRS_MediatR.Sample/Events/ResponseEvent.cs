using HR.CRUDWebApi.CQRS_MediatR.Sample.Models;
using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Events
{
    public record ResponseEvent(ResponseDto Response) : INotification;
}
