using Microsoft.EntityFrameworkCore;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Data;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Login;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Login;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Users.Entities;
using System.Security.Claims;
using Tars.Contracts.Adapter.Authorization;
using Tars.Contracts.Adapter.Authorization.Dtos;
using Tars.Contracts.Cqrs;
using Tars.Core.Cqrs;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Logic.Modules.Auth.Cqrs.Login
{
    public class LoginUserCommandHandler : AbstractCommandHandler<LoginUserCommand, LoginUserOutputDto>, ILoginUserCommandHandler
    {
        protected const string INVALID_USERNAME_OR_PASSWORD_MESSAGE = "Invalid username or password.";

        protected readonly AppDbContext _dbContext;

        protected readonly IAuthService _authService;
        protected readonly IPasswordHasher _passwordHasher;

        public LoginUserCommandHandler(AppDbContext dbContext, IAuthService authService, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        protected override async Task<ICommandResult<LoginUserOutputDto>> HandleAsync(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User? foundUser = await FindUser(request.Input.Username, cancellationToken);

            if (foundUser is null)
            {
                return Fail(MountOutputDto(null), INVALID_USERNAME_OR_PASSWORD_MESSAGE);
            }

            bool isPasswordValid = _passwordHasher.VerifyPassword(request.Input.Password, foundUser.PasswordSalt, foundUser.Password);

            if (!isPasswordValid)
            {
                return Fail(MountOutputDto(null), INVALID_USERNAME_OR_PASSWORD_MESSAGE);
            }

            ICollection<AuthTicketClaimDto> claims = BuildUserClaims(foundUser);
            IAuthTicket authTicket = _authService.CreateAuthTicket(claims, Convert.ToString(foundUser.Id));

            return Success(MountOutputDto(authTicket));
        }

        protected async Task<User?> FindUser(string username, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower(), cancellationToken);
        }

        protected ICollection<AuthTicketClaimDto> BuildUserClaims(User user)
        {
            ICollection<AuthTicketClaimDto> claims = new List<AuthTicketClaimDto>()
            {
                new AuthTicketClaimDto()
                {
                    ClaimName = nameof(User.Id),
                    ClaimType = ClaimValueTypes.String,
                    ClaimValue = Convert.ToString(user.Id)
                },
                new AuthTicketClaimDto()
                {
                    ClaimName = nameof(User.Username),
                    ClaimType = ClaimValueTypes.String,
                    ClaimValue = Convert.ToString(user.Username)
                },
                new AuthTicketClaimDto()
                {
                    ClaimName = nameof(User.Email),
                    ClaimType = ClaimValueTypes.String,
                    ClaimValue = Convert.ToString(user.Email)
                },
                new AuthTicketClaimDto()
                {
                    ClaimName = nameof(User.Name),
                    ClaimType = ClaimValueTypes.String,
                    ClaimValue = Convert.ToString(user.Name)
                },
                new AuthTicketClaimDto()
                {
                    ClaimName = nameof(User.Role),
                    ClaimType = ClaimValueTypes.Integer32,
                    ClaimValue = Convert.ToString((int)user.Role)
                }
            };

            return claims;
        }

        protected LoginUserOutputDto MountOutputDto(IAuthTicket? authTicket)
        {
            return new LoginUserOutputDto() { AuthTicket = authTicket };
        }
    }
}
