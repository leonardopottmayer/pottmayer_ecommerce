using MediatR;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Data;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Register;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Users.Entities;
using Pottmayer.Ecommerce.Shared.Core.Domain.Modules.Users.Enums;
using Tars.Contracts.Adapter.Authorization;
using Tars.Contracts.Cqrs;
using Tars.Core.Cqrs;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Logic.Modules.Auth.Cqrs.Register
{
    public class RegisterUserCommandHandler : AbstractCommandHandler<RegisterUserCommand, RegisterUserOutputDto>, IRegisterUserCommandHandler
    {
        protected const string FAILED_TO_REGISTER_USER_MESSAGE = "Failed to register user.";

        protected readonly AppDbContext _dbContext;

        protected readonly IAuthService _authService;
        protected readonly IPasswordHasher _passwordHasher;
        protected readonly IMediator _mediator;

        public RegisterUserCommandHandler(AppDbContext dbContext, IAuthService authService, IPasswordHasher passwordHasher, IMediator mediator)
        {
            _dbContext = dbContext;
            _authService = authService;
            _passwordHasher = passwordHasher;
            _mediator = mediator;
        }

        protected override async Task<ICommandResult<RegisterUserOutputDto>> HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var validationCmdInput = new ValidateUserRegistrationDataInputDto()
            {
                Username = request.Input.Username,
                Email = request.Input.Email,
                Name = request.Input.Name,
                Password = request.Input.Password,
                PasswordConfirmation = request.Input.PasswordConfirmation
            };

            var validationCmd = new ValidateUserRegistrationDataCommand(validationCmdInput);
            var validationCmdResult = await _mediator.Send(validationCmd);

            if (!validationCmdResult.Success)
            {
                return Fail(MountOutputDto(null, validationCmdResult.Output!.ValidationErrors));
            }

            User? insertedUser = await CreateUser(request.Input);

            if (insertedUser is null)
            {
                return Fail(MountOutputDto(null, new List<string>() { FAILED_TO_REGISTER_USER_MESSAGE }));
            }

            return Success(MountOutputDto(insertedUser, new List<string>()));
        }

        protected async Task<User?> CreateUser(RegisterUserInputDto input)
        {
            string hashedPassword = _passwordHasher.HashPassword(input.Password, out byte[] passwordSalt);

            var newUser = new User
            {
                Id = default,
                Key = Guid.NewGuid(),
                Name = input.Name,
                Username = input.Username,
                Email = input.Email,
                Role = UserRole.Default,
                Status = UserStatus.WaitingConfirmation,
                Password = hashedPassword,
                PasswordSalt = Convert.ToBase64String(passwordSalt),
            };

            await _dbContext.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return newUser;
        }

        protected RegisterUserOutputDto MountOutputDto(User? user, List<string> errorMessages)
        {
            return new RegisterUserOutputDto()
            {
                User = user,
                ErrorMessages = errorMessages
            };
        }
    }
}
