using HR.CRUDWebApi.CQRS_MediatR.Sample.Entity;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Models
{
    public class ResponseDto
    {
        public Guid UserID { get; set; }
        public string ActionMessage { get; set; }
        public ResponseDto() { }
        public ResponseDto(Guid userId, string actionMessage)
        {
            UserID = userId;
            ActionMessage = actionMessage;
        }
    }

}
