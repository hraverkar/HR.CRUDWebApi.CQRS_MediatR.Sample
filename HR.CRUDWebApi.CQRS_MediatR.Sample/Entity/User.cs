
using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity.BaseEntity;
using System.ComponentModel.DataAnnotations;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Entity
{
    public class User : BEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Department { get; set; }
    }
}
