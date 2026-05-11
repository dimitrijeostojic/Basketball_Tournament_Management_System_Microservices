using Domain.Enums;

namespace Application.Knockout.GetKnockoutBracket;

public sealed class KnockoutMatchDto
{
    public Guid PublicId { get; set; }
    public KnockoutRound Round { get; set; }
    public int MatchOrder { get; set; }
    public Guid? HomeTeamPublicId { get; set; }
    public string? HomeTeamName { get; set; }
    public Guid? AwayTeamPublicId { get; set; }
    public string? AwayTeamName { get; set; }
    public int? HomePoints { get; set; }
    public int? AwayPoints { get; set; }
    public Guid? WinnerPublicId { get; set; }
    public Guid? StadiumPublicId { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public KnockoutMatchStatus Status { get; set; }
}
