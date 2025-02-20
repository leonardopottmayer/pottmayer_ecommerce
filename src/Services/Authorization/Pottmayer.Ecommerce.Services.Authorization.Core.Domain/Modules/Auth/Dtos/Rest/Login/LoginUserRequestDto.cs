using Tars.Contracts;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Rest.Login
{
    public class LoginUserRequestDto : IDataTransferObject
    {
        public required string? Username { get; set; }
        public required string Password { get; set; }
    }
}
