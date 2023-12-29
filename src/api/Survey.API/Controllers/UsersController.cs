using API.Controllers;
using Dapr.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Survey.API.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly DaprClient _daprClient;

    public UsersController(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<GenericResponse<LoginResponse>> Login(LoginRequest model)
    {
        var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Post, "SurveyService", "/Users/login", model);
        var response = await _daprClient.InvokeMethodAsync<GenericResponse<LoginResponse>>(request);
        return response;
    }

}
