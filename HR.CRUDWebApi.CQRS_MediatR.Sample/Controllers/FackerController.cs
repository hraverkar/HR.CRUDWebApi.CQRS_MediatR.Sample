using HR.CRUDWebApi.CQRS_MediatR.Sample.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FackerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public FackerController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
