using Application.User.Delete;
using Application.User.GetAll;
using AuthService.Extensions;
using Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class UserController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllUsersRequest(), cancellationToken);
        return result.ToActionResult();
    }

    [HttpDelete("{userId:Guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new DeleteUserRequest(userId), cancellationToken);
        return result.ToActionResult();
    }
}