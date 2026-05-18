using Core;
using MediatR;

namespace Application.Stadium.Create;

public sealed record CreateStadiumRequest(string StadiumName, string City, int Capacity)
    : IRequest<Result<CreateStadiumResponse>>;
