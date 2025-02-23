namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Models
{
    public record UserDto
    {
        public Guid UserID { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Department { get; init; }
        public string? Password { get; init; }
    }
}
