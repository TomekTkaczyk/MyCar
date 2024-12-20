using MyCar.Shared.Abstractions;

namespace MyCar.Shared.Infrastructure.Time;
internal class UtcClock : IClock
{
	public DateTime CurrentDate() => DateTime.UtcNow;
}
