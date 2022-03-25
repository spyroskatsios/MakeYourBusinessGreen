namespace MakeYourBusinessGreen.Application.Commands.AuthCommands;
public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<AuthResponse>
{

}
