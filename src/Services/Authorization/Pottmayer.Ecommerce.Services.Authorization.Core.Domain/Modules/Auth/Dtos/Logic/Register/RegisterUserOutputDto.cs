using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Users.Entities;
using Tars.Contracts;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register
{
    public class RegisterUserOutputDto : IDataTransferObject
    {
        public User? User { get; set; }
        public List<string> ErrorMessages { get; set; } = [];
    }
}
