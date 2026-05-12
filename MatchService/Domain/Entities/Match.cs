using Domain.Enums;

namespace Domain.Entities;

public sealed class Match : Entity
{
    public Guid HomeTeamPublicId { get; private set; }
    public Guid AwayTeamPublicId { get; private set; }
    public Guid StadiumPublicId { get; private set; }
    public string HomeTeamName { get; private set; } = string.Empty;
    public string AwayTeamName { get; private set; } = string.Empty;
    public string StadiumName { get; private set; } = string.Empty;
    public DateTime StartTime { get; private set; }
    public MatchStatus Status { get; private set; }
    public int? HomePoints { get; private set; }
    public int? AwayPoints { get; private set; }
    public bool IsForfeit { get; private set; }
    public ForfeitSide? ForfeitLoser { get; private set; }

    private const int _forfeitWinnerPoints = 20;
    private const int _forfeitLoserPoints = 0;
    private const int _winStandingPoints = 2;
    private const int _lossStandingPoints = 1;


    public Match UpdateStartTime(DateTime? startTime)
    {
        EnsureNotFinishedOrCancelled();
        if (startTime != null)
        {
            StartTime = startTime.Value;
        }
        return this;
    }

    public Match UpdateHomeTeam(Guid? homeTeamPublicId, string? homeTeamName)
    {
        EnsureNotFinishedOrCancelled();
        if (homeTeamPublicId != null)
        {
            HomeTeamPublicId = homeTeamPublicId.Value;
        }
        if (homeTeamName != null)
        {
            HomeTeamName = homeTeamName;
        }
        return this;
    }
    public Match UpdateAwayTeam(Guid? awayTeamPublicId, string? awayTeamName)
    {
        EnsureNotFinishedOrCancelled();
        if (awayTeamPublicId != null)
        {
            AwayTeamPublicId = awayTeamPublicId.Value;
        }
        if (awayTeamName != null)
        {
            AwayTeamName = awayTeamName;
        }
        return this;
    }

    public Match UpdateStadium(Guid? stadiumPublicId, string? stadiumName)
    {
        EnsureNotFinishedOrCancelled();
        if (stadiumPublicId != null)
        {
            StadiumPublicId = stadiumPublicId.Value;
        }
        if (stadiumName != null)
        {
            StadiumName = stadiumName;
        }
        return this;
    }

    public static Match Schedule(
        Guid homeTeamPublicId,
        Guid awayTeamPublicId,
        Guid stadiumPublicId,
        string homeTeamName,
        string awayTeamName,
        string stadiumName,
        DateTime startTime)
    {
        if (homeTeamPublicId == awayTeamPublicId)
            throw new InvalidOperationException("Home and away team must be different.");

        if (startTime == default)
            throw new ArgumentException("StartTime is required.", nameof(startTime));

        return new Match
        {
            HomeTeamPublicId = homeTeamPublicId,
            AwayTeamPublicId = awayTeamPublicId,
            StadiumPublicId = stadiumPublicId,
            HomeTeamName = homeTeamName,
            AwayTeamName = awayTeamName,
            StadiumName = stadiumName,
            StartTime = startTime,
            Status = MatchStatus.Scheduled
        };
    }

    public void RecordResult(DateTime now, int homePoints, int awayPoints)
    {
        EnsureNotFinishedOrCancelled();

        if (now < StartTime)
            throw new InvalidOperationException("Cannot record result before StartTime.");

        if (homePoints < 0 || awayPoints < 0)
            throw new InvalidOperationException("Points cannot be negative.");

        if (homePoints == awayPoints)
            throw new InvalidOperationException("A basketball match cannot end in a draw.");

        if (Status is not MatchStatus.Scheduled)
            throw new InvalidOperationException("Result can only be recorded for Scheduled matches.");

        HomePoints = homePoints;
        AwayPoints = awayPoints;
        IsForfeit = false;
        ForfeitLoser = null;
        Status = MatchStatus.Completed;
    }

    public void Forfeit(ForfeitSide loserSide)
    {
        EnsureNotFinishedOrCancelled();

        IsForfeit = true;
        ForfeitLoser = loserSide;

        if (loserSide == ForfeitSide.Home)
        {
            HomePoints = _forfeitLoserPoints;
            AwayPoints = _forfeitWinnerPoints;
        }
        else
        {
            HomePoints = _forfeitWinnerPoints;
            AwayPoints = _forfeitLoserPoints;
        }

        Status = MatchStatus.Completed;
    }

    public MatchStandingsDelta CalculateStandingsDelta()
    {
        if (Status != MatchStatus.Completed)
            throw new InvalidOperationException("Standings delta can only be calculated for completed matches.");

        if (HomePoints is null || AwayPoints is null)
            throw new InvalidOperationException("Completed match must have points.");

        var homeFor = HomePoints.Value;
        var homeAgainst = AwayPoints.Value;
        var awayFor = AwayPoints.Value;
        var awayAgainst = HomePoints.Value;

        var homeWin = homeFor > homeAgainst;
        var awayWin = awayFor > awayAgainst;

        return new MatchStandingsDelta(
            HomeTeamPublicId: HomeTeamPublicId,
            AwayTeamPublicId: AwayTeamPublicId,
            HomePlayed: 1,
            HomeWins: homeWin ? 1 : 0,
            HomeLosses: homeWin ? 0 : 1,
            HomePointsFor: homeFor,
            HomePointsAgainst: homeAgainst,
            HomeStandingPoints: homeWin ? _winStandingPoints : _lossStandingPoints,
            AwayPlayed: 1,
            AwayWins: awayWin ? 1 : 0,
            AwayLosses: awayWin ? 0 : 1,
            AwayPointsFor: awayFor,
            AwayPointsAgainst: awayAgainst,
            AwayStandingPoints: awayWin ? _winStandingPoints : _lossStandingPoints
        );
    }

    private void EnsureNotFinishedOrCancelled()
    {
        if (Status is MatchStatus.Completed or MatchStatus.Cancelled)
            throw new InvalidOperationException("Match is already finished or cancelled.");
    }
}

public sealed record MatchStandingsDelta(
    Guid HomeTeamPublicId,
    Guid AwayTeamPublicId,
    int HomePlayed, int HomeWins, int HomeLosses,
    int HomePointsFor, int HomePointsAgainst, int HomeStandingPoints,
    int AwayPlayed, int AwayWins, int AwayLosses,
    int AwayPointsFor, int AwayPointsAgainst, int AwayStandingPoints
);
