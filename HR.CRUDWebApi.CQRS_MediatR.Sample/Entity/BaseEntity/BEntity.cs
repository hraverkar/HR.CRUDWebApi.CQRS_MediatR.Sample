using System.ComponentModel.DataAnnotations;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Entity.BaseEntity
{
    public class BEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
