using Core;

namespace Application.Common;

public static class ApplicationErrors
{
    public static readonly Error NotFound =
        new("Match.NotFound", "The requested resource was not found.");

    public static readonly Error DifferentTeams =
        new("Match.DifferentTeams", "Home and away team must be different.");
    public static readonly Error TeamConflict =
        new("Match.TeamConflict", "One of the teams already has a match at that time.");
    public static readonly Error StadiumConflict =
        new("Match.StadiumConflict", "Stadium is already occupied at that time.");
    public static readonly Error SameMatchExists =
        new("Match.SameMatchExists", "A match between these teams already exists in this group.");
    public static readonly Error DeleteFailed =
     new("Match.DeleteFailed", "Failed to delete the match.");
    public static readonly Error InvalidOperation =
     new("Match.InvalidOperation", "Invalid operation.");


    public static readonly Error KnockoutMatchNotScheduled =
        new("Knockout.MatchNotScheduled", "The knockout match is not scheduled yet — teams have not been assigned.");
    public static readonly Error InvalidSeededTeamsCount =
        new("Knockout.InvalidSeededTeamsCount", "Exactly 8 seeded teams are required to create a knockout bracket.");
    public static readonly Error DuplicateSeededTeams =
        new("Knockout.DuplicateSeededTeams", "Seeded teams must all be different.");

}