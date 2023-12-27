using Dapr.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Survey.Shared.Models;

namespace Survey.API.Controllers
{
    [ApiController]
    public class SurveyController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<SurveyController> logger;

        public SurveyController(DaprClient daprClient, ILogger<SurveyController> logger)
        {
            _daprClient = daprClient;
            this.logger = logger;
        }

        [HttpPost]
        [Route("survey-assignment")]
        public async Task<GenericResponse<string>> SurveyAssignment(ProcessSurveyRequest request)
        {
            var response = await _daprClient.InvokeMethodAsync<GenericResponse<string>>(HttpMethod.Post, "SurveyService", "/Survey/survey-assignment");
            return response;
        }


        [HttpPost]
        [Route("vote-the-survey")]
        public async Task VoteTheSurvey(SurveyQuestionRequest request)
        {
            var response = await _daprClient.InvokeMethodAsync<GenericResponse<string>>(HttpMethod.Post, "SurveyService", "/Survey/vote-the-survey");
        }
    }
}