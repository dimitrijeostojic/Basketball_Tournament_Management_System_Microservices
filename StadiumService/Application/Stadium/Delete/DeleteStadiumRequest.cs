using Core;
using MediatR;

namespace Application.Stadium.Delete;

public sealed record DeleteStadiumRequest(Guid PublicId) : IRequest<Result<DeleteStadiumResponse>>;
