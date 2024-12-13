using Microsoft.IdentityModel.JsonWebTokens;
using MyCar.Shared.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCar.Shared.Infrastructure.Auth;
internal class JwtDecoder
{
	private static void DecodeJwtToken(string token)
	{
		var handler = new JsonWebTokenHandler();

		if(!handler.CanReadToken(token)) {
			throw new InvalidJwtException();
		}

		var jsonToken = handler.ReadJsonWebToken(token);

		Console.WriteLine("Claims from JWT token:");
		foreach(var claim in jsonToken.Claims) {
			Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
		}

		// Jeśli potrzebujesz konkretnego claimu:
		var sub = jsonToken.GetClaim("sub");
		Console.WriteLine($"Subject (sub): {sub}");
	}
}
