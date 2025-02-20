using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register;
using Tars.Contracts.Cqrs;
using Tars.Core.Cqrs;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Register
{
    public class ValidateUserRegistrationDataCommand : AbstractCommand<ValidateUserRegistrationDataInputDto, ValidateUserRegistrationDataOutputDto>
    {
        public ValidateUserRegistrationDataCommand(ValidateUserRegistrationDataInputDto input) : base(input) { }
    }

    public interface IValidateUserRegistrationDataCommandHandler : ICommandHandler<ValidateUserRegistrationDataCommand, ValidateUserRegistrationDataOutputDto> { }
}
