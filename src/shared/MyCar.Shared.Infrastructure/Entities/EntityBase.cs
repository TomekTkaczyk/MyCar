namespace MyCar.Shared.Infrastructure.Entities;
public abstract class EntityBase
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }

}
