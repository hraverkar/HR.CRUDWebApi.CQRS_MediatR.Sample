using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Events
{
    public class LogEventHandler : INotificationHandler<ResponseEvent>, INotificationHandler<ErrorEvent>
    {
        public async Task Handle(ResponseEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"User ID: {notification.Response.UserID} , Message: {notification.Response.ActionMessage}");
        }

        public async Task Handle(ErrorEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error ID: {notification.Response.UserID} , Message: {notification.Response.ActionMessage}");
        }
    }
}
