namespace Application.Stadium.Create;

public sealed record CreateStadiumResponse
{
    public Guid PublicId { get; set; }
    public string StadiumName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Capacity { get; set; }
}
