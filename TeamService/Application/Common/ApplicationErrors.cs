using Core;

namespace Application.Common;

public static class ApplicationErrors
{
    public static readonly Error NotFound = new("Application.NotFound", "The requested resource was not found.");
    public static readonly Error MaxGroupsReached = new("Group.MaxGroupsReached", "Cannot create more than 4 groups.");
    public static readonly Error GroupTeamLimitReached = new("Group.TeamLimitReached", "Cannot create more than 4 teams in a group.");
    public static readonly Error MultipleTeamsError = new("Team.MultipleTeamsError", "A team with the same name already exists.");
}
