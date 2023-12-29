using API.Controllers;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;

           // var requestData = new Dictionary<string, string>();
           // var accessToken = Request.Headers[HeaderNames.Authorization];
           //// var accessToken = await HttpContext.GetTokenAsync("access_token");
           // requestData.Add("Authorization", UserService.Token);

            request.UserId = UserService.UserId;
            await _daprClient.PublishEventAsync("pubsub", "survey-assignment", request, cancellationToken);
            return GenericResponse<string>.Success(request.SurveyItemId, 200);
        }


        [HttpPost]
        [Route("vote-the-survey")]
        public async Task VoteTheSurvey(SurveyQuestionRequest request)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken cancellationToken = source.Token;
            request.UserId = UserService.UserId;
            await _daprClient.PublishEventAsync("pubsub", "vote-the-survey", request, cancellationToken);
        }
    }
}