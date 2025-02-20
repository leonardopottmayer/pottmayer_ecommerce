using Tars.Contracts;
using Tars.Contracts.Adapter.Authorization;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Login
{
    public class LoginUserOutputDto : IDataTransferObject
    {
        public IAuthTicket? AuthTicket { get; set; }
    }
}
