using API.Controllers;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace Survey.API.Controllers
{
    public class SurveyController : ApiControllerBase
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
            await _daprClient.PublishEventAsync("pubsub", "survey-assignment", request);
            return GenericResponse<string>.Success(request.SurveyItemId, 200);
        }


        [HttpPost]
        [Route("vote-the-survey")]
        public async Task VoteTheSurvey(SurveyQuestionRequest request)
        {
            await _daprClient.PublishEventAsync("pubsub", "vote-the-survey", request);
        }
    }
}