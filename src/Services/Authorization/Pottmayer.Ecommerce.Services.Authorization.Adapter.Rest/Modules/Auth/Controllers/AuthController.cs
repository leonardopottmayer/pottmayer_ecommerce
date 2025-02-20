using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Login;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Cqrs.Register;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Login;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Logic.Register;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Rest.Login;
using Pottmayer.Ecommerce.Services.Authorization.Core.Domain.Modules.Auth.Dtos.Rest.Register;
using Tars.Adapter.Rest.Controllers;
using Tars.Adapter.Rest.Extensions;
using Tars.Contracts.Adapter.Rest;
using Tars.Contracts.Adapter.Rest.Dtos;

namespace Pottmayer.Ecommerce.Services.Authorization.Adapter.Rest.Modules.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : DefaultController
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            var cmd = new RegisterUserCommand(_mapper.Map<RegisterUserInputDto>(request));

            var result = await _mediator.Send(cmd);

            if (result.Success)
            {
                return Ok(new { }).WithSuccessIndicator(true);
            }

            List<ResponseErrorItemDto> responseErrors = result.Output!.ErrorMessages
                .Select(x => new ResponseErrorItemDto() { Code = 0, Message = x })
                .ToList();

            return UnprocessableEntity(new { }).WithErrors(responseErrors);
        }

        [HttpPost("login")]
        [ProducesResponseType(201, Type = typeof(IApiResponse<LoginUserResponseDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto request)
        {
            var cmd = new LoginUserCommand(_mapper.Map<LoginUserInputDto>(request));

            var result = await _mediator.Send(cmd);

            if (result.Success)
            {
                return Ok(new LoginUserResponseDto() { JwtToken = result.Output!.AuthTicket?.JwtToken ?? string.Empty })
                      .WithSuccessIndicator(true);
            }

            return UnprocessableEntity(new { }).WithMessage(result.Message ?? "Failed to authenticate.");
        }
    }
}
