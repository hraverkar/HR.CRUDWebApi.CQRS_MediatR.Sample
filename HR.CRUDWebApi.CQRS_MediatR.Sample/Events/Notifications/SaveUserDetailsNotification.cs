using MediatR;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Events.Notifications
{
    public record SaveUserDetailsNotification : INotification
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public SaveUserDetailsNotification(Guid id, string firstName, string email)
        {
            Id = id;
            FirstName = firstName;
            Email = email;
        }

    }
}
