using Dapr;
using Survey.Application.Worker;

namespace Survey.Service.Controllers;

[ApiController]
public class SurveyController : ApiControllerBase
{
    private readonly ILogger<SurveyController> logger;

    public SurveyController(ILogger<SurveyController> logger)
    {
        this.logger = logger;
    }

    [HttpPost]
    [Topic("pubsub", "survey-assignment")]
    [Route("survey-assignment")]
    public async Task<GenericResponse<string>> SurveyAssignment(ProcessSurveyRequest request)
    {
        var instanceId = Guid.NewGuid().ToString();
        logger.LogInformation($"survey-assignment : {instanceId} message from SurveyService");

        await Mediator.Send(new SurveyAssignmentCommand
        {
            Model = request,
            UserId = request.UserId,
            InstanceId = instanceId,
            SurveyItemId = request.SurveyItemId,
        });

        return GenericResponse<string>.Success(instanceId, 200);
    }

    [HttpPost]
    [Topic("pubsub", "vote-the-survey")]
    [Route("vote-the-survey")]
    public async Task VoteTheSurvey(SurveyQuestionRequest request)
    {
        
       await Mediator.Send(new SurveyConfirmationCommand
       {
           Model = request,           
       });
    }
}