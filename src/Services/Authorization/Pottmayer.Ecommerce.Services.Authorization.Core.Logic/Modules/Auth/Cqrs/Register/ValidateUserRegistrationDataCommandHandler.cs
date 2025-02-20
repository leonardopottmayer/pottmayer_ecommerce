using Microsoft.EntityFrameworkCore;
using Pottmayer.Ecommerce.Services.Authorization.Adapter.Data;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Register;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register;
using System.Text.RegularExpressions;
using Tars.Contracts.Cqrs;
using Tars.Core.Cqrs;

namespace Pottmayer.Ecommerce.Services.Authorization.Core.Logic.Modules.Auth.Cqrs.Register
{
    public class ValidateUserRegistrationDataCommandHandler : AbstractCommandHandler<ValidateUserRegistrationDataCommand, ValidateUserRegistrationDataOutputDto>, IValidateUserRegistrationDataCommandHandler
    {
        protected const string ALREADY_IN_USE_MESSAGE = "{0} already in use.";
        protected const string PASSWORD_CANNOT_BE_EMPTY_MESSAGE = "Password cannot be empty.";
        protected const string PASSWORD_CONFIRMATION_CANNOT_BE_EMPTY_MESSAGE = "Password confirmation cannot be empty.";
        protected const string PASSWORDS_DO_NOT_MATCH_MESSAGE = "Passwords do not match.";
        protected const string PASSWORDS_ARE_NOT_STRONG_ENOUGH_MESSAGE = "Password is not strong enough.";

        protected readonly AppDbContext _dbContext;

        public ValidateUserRegistrationDataCommandHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task<ICommandResult<ValidateUserRegistrationDataOutputDto>> HandleAsync(ValidateUserRegistrationDataCommand request, CancellationToken cancellationToken)
        {
            var errorMessages = new List<string>();

            await ValidateUsernameAlreadyInUse(errorMessages, request.Input.Username);
            await ValidateEmailAlreadyInUse(errorMessages, request.Input.Email);

            ValidatePassword(errorMessages, request.Input.Password, request.Input.PasswordConfirmation);

            var output = new ValidateUserRegistrationDataOutputDto()
            {
                ValidationErrors = errorMessages
            };

            return errorMessages.Any() ? Fail(output) : Success(output);
        }

        protected async Task ValidateUsernameAlreadyInUse(List<string> errorMessages, string username)
        {
            bool usernameInUse = await _dbContext.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());

            if (usernameInUse)
            {
                errorMessages.Add(string.Format(ALREADY_IN_USE_MESSAGE, "Username"));
            }
        }

        protected async Task ValidateEmailAlreadyInUse(List<string> errorMessages, string email)
        {
            bool emailInUse = await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

            if (emailInUse)
            {
                errorMessages.Add(string.Format(ALREADY_IN_USE_MESSAGE, "Email"));
            }
        }

        protected void ValidatePassword(List<string> errorMessages, string password, string passwordConfirmation)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                errorMessages.Add(PASSWORD_CANNOT_BE_EMPTY_MESSAGE);
            }

            if (string.IsNullOrEmpty(passwordConfirmation) || string.IsNullOrWhiteSpace(passwordConfirmation))
            {
                errorMessages.Add(PASSWORD_CONFIRMATION_CANNOT_BE_EMPTY_MESSAGE);
            }

            if (password != passwordConfirmation)
            {
                errorMessages.Add(PASSWORDS_DO_NOT_MATCH_MESSAGE);
            }

            var regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$");
            bool strongPassword = regex.IsMatch(password);

            if (!strongPassword)
            {
                errorMessages.Add(PASSWORDS_ARE_NOT_STRONG_ENOUGH_MESSAGE);
            }
        }
    }
}
