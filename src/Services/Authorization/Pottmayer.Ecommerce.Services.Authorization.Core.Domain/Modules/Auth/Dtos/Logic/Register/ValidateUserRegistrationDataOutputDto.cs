using Tars.Contracts;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register
{
    public class ValidateUserRegistrationDataOutputDto : IDataTransferObject
    {
        public List<string> ValidationErrors { get; set; } = [];
    }
}
