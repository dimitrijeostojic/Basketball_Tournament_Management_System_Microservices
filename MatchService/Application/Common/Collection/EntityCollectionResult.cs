namespace Application.Common.Collection;

public abstract class EntityCollectionResult<T> where T : class
{
    public IReadOnlyCollection<T> Items { get; set; }
    public int Count { get; set; }

    protected EntityCollectionResult(ICollection<T> items)
    {
        Items = [.. items];
        Count = items.Count;
    }
}
