using HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Queries;
using HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            var result = await _mediator.Send(new GetUserListQuery());
            return Ok(result);
        }

        [HttpGet("GetUserDetails/{UserID}")]
        public async Task<IActionResult> GetUserDetails(Guid UserID)
        {
            var result = await _mediator.Send(new GetUserDetailsByUserIDQuery(UserID));
            return Ok(result);
        }

        [HttpDelete("DeleteUserDetails/{UserID}")]
        public async Task<IActionResult> DeleteUserDetails(Guid UserID)
        {
            var result = await _mediator.Send(new DeleteUserDetailsCommand(UserID));
            return Ok(result);
        }

        [HttpPost("SaveUserDetails")]
        public async Task<IActionResult> SaveUserDetails(SaveUserDetailsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
