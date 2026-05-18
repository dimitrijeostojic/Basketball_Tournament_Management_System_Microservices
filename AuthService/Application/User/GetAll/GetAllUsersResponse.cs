namespace Application.User.GetAll;

public sealed record GetAllUsersResponse
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public ICollection<string> Roles { get; set; } = [];
}
