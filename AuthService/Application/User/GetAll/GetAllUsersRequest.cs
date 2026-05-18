using Core;
using MediatR;

namespace Application.User.GetAll;

public sealed record GetAllUsersRequest
    : IRequest<Result<List<GetAllUsersResponse>>>;
