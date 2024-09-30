namespace Auth.Domain.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; }
}
