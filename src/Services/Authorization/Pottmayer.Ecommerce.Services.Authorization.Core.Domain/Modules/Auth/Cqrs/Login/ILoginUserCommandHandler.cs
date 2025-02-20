using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Login;
using Tars.Contracts.Cqrs;
using Tars.Core.Cqrs;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Login
{
    public class LoginUserCommand : AbstractCommand<LoginUserInputDto, LoginUserOutputDto>
    {
        public LoginUserCommand(LoginUserInputDto input) : base(input) { }
    }

    public interface ILoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserOutputDto> { }
}
