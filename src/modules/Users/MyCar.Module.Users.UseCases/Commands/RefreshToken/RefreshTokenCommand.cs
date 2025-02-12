using MediatR;
using MyCar.Shared.Abstractions.Auth;

namespace MyCar.Module.Users.UseCases.Commands.RefreshToken;
internal sealed record RefreshTokenCommand(string Token) : IRequest<JsonWebToken> { }
