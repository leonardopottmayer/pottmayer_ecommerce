using Tars.Contracts;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Login
{
    public class LoginUserInputDto : IDataTransferObject
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
