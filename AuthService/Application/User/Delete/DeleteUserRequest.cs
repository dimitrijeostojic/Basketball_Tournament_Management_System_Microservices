using Core;
using MediatR;

namespace Application.User.Delete;

public sealed record DeleteUserRequest(Guid UserId)
    : IRequest<Result<DeleteUserResponse>>;
