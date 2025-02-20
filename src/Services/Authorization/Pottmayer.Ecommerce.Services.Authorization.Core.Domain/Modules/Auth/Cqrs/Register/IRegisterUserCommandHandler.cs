using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register;
using Tars.Contracts.Cqrs;
using Tars.Core.Cqrs;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Register
{
    public class RegisterUserCommand : AbstractCommand<RegisterUserInputDto, RegisterUserOutputDto>
    {
        public RegisterUserCommand(RegisterUserInputDto input) : base(input) { }
    }

    public interface IRegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserOutputDto> { }
}
