namespace Survey.Service.Controllers;

[ApiController]
public class SurveyController : ApiControllerBase
{
    [HttpPost]
    [Route("survey-assignment")]
    public async Task<GenericResponse<string>> SurveyAssignment(ProcessSurveyRequest request)
    {
        var instanceId = Guid.NewGuid().ToString();
        await Mediator.Send(new ProcessSurveyCommand
        {
            ProcessSurveyRequest = request,
            UserId = UserService.UserId,
            InstanceId = instanceId,
        });

        return GenericResponse<string>.Success(instanceId, 200);
    }

    [HttpPost]
    [Route("vote-the-survey")]
    public async Task VoteTheSurvey(SurveyQuestionRequest request)
    {
        await Mediator.Send(new VoteTheSurveyCommand
        {
            Model = request,
            UserId = UserService.UserId,
        });
    }
}