using MediatR;

namespace MyCar.Module.Users.UseCases.Commands.UpdateClaims;
internal sealed record UpdateClaimsCommand : IRequest
{
	public Guid Id { get; set; }
	public string Role { get; set; }
	public bool IsActive { get; set; }
	public IDictionary<string, IEnumerable<string>> Claims { get; set; }
}
