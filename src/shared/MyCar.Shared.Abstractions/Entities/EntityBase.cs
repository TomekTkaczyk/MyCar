﻿namespace MyCar.Shared.Abstractions.Entities;
public abstract class EntityBase
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }

}
