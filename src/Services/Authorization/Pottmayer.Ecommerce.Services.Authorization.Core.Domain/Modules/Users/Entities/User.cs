using Pottmayer.Ecommerce.Shared.Core.Domain.Modules.Users.Enums;
using Tars.Contracts;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Users.Entities
{
    public sealed class User : IEntity
    {
        public required long Id { get; set; }
        public required Guid Key { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordSalt { get; set; }
        public required UserRole Role { get; set; }
        public required UserStatus Status { get; set; }
    }
}
