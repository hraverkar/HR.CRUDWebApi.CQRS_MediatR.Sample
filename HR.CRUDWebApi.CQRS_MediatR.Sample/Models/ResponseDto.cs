using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Models
{
    public class ResponseDto
    {
        public Guid Id { get; set; }
        public string ActionMessage { get; set; }
        public ResponseDto() { }
        public ResponseDto(Guid id, string actionMessage)
        {
            Id = id;
            ActionMessage = actionMessage;
        }
    }

}
