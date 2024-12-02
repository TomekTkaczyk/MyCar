namespace MyCar.Shared.Abstractions.Contexts;

public interface IIdentityContext
{
	Guid Id { get; }
	string Role { get; }
	bool IsAuthenticated { get; }
	Dictionary<string, IEnumerable<string>> Claims { get; }
}