namespace ShopRadar.Domain.Abstractions;

public abstract class Entity<TId> where TId : class
{
    protected Entity(TId id)
    {
        Id = id ?? throw new Exception("Id can't be null");
    }

    protected Entity()
    {
    }

    public TId Id { get; init; }
}