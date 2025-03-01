using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Events
{
    public class LogEventHandler : INotificationHandler<ResponseEvent>, INotificationHandler<ErrorEvent>
    {
        Task INotificationHandler<ResponseEvent>.Handle(ResponseEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"User ID: {notification.Response.Id} , Message: {notification.Response.ActionMessage}");
            return Task.CompletedTask;
        }

        Task INotificationHandler<ErrorEvent>.Handle(ErrorEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"User ID: {notification.Response.Id} , Message: {notification.Response.ActionMessage}");
            return Task.CompletedTask;
        }
    }
}
