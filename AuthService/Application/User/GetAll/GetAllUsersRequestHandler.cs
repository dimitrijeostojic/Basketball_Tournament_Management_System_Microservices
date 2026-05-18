using Core;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.User.GetAll;

internal sealed class GetAllUsersRequestHandler(
    UserManager<Domain.Entities.User> userManager)
    : IRequestHandler<GetAllUsersRequest, Result<List<GetAllUsersResponse>>>
{
    private readonly UserManager<Domain.Entities.User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<Result<List<GetAllUsersResponse>>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();

        var response = new List<GetAllUsersResponse>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            response.Add(new GetAllUsersResponse
            {
                UserId = Guid.Parse(user.Id),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email,
                Roles = roles
            });
        }

        return Result<List<GetAllUsersResponse>>.Success(response);
    }
}
