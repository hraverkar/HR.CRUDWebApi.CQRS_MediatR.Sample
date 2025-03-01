namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Models
{
    public record UserDto
    {
        public Guid Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? Department { get; init; }
        public string? Password { get; init; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

    }

    public record UserInfo
    {
        public int Count { get; init; }
        public List<UserDto>? UserDto { get; init; }
    }
}
