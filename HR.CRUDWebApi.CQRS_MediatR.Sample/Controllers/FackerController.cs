using HR.CRUDWebApi.CQRS_MediatR.Sample.Segrigation.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FackerController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // Wanted to add fake users to the table

        [HttpPost("FackerPost")]
        public async Task<IActionResult> FackerPost()
        {
            var command = new FakeUserAdditionCommand();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
