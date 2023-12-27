using Microsoft.AspNetCore.Authorization;
using Survey.Application;

namespace Survey.Service.Controllers;

[ApiController]
public class UsersController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<GenericResponse<LoginResponse>> Login(LoginRequest model)
    {
        var command = Mapper.Map<LoginCommand>(model);

        var response = await Mediator.Send(command);
        return response;
    }

}
