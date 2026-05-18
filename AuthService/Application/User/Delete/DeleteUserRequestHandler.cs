using Application.Common;
using Core;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.Delete;

internal sealed class DeleteUserRequestHandler(
    UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<DeleteUserRequest, Result<DeleteUserResponse>>
{
    private readonly UserManager<Domain.Entities.User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<Result<DeleteUserResponse>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            return Result<DeleteUserResponse>.Failure(ApplicationErrors.NotFound);
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return Result<DeleteUserResponse>.Failure(ApplicationErrors.DeleteFailure);
        }

        return Result<DeleteUserResponse>.Success(new DeleteUserResponse { Message = "User deleted successfully." });
    }
}
