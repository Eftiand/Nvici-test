
namespace API.Data.Entities;

public abstract class Entity<TId>
{
    protected Entity(TId? id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public TId? Id { get; init; }
}