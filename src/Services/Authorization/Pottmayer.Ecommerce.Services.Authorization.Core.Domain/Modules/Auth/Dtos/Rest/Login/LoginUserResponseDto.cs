using Tars.Contracts;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Rest.Login
{
    public class LoginUserResponseDto : IDataTransferObject
    {
        public required string JwtToken { get; set; }
    }
}
