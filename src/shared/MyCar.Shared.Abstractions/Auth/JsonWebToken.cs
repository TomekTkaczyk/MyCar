namespace MyCar.Shared.Abstractions.Auth;

public class JsonWebToken
{
	public string AccesToken { get; set; }
	public RefreshToken RefreshToken { get; set; }
}
