using Application.Common.Interfaces;
using Domain.Enums;
using MediatR;

namespace Survey.Application.Worker;

public class SurveyAssignmentCommand  :IRequest<GenericResponse<SurveyAssignmentResponse>>
{
    public ProcessSurveyRequest Model { get; set; }
    public string UserId {  get; set; }
    public string SurveyItemId { get; set; }
    public string InstanceId { get; set; }
}

public class SurveyAssignmentCommandHandler : IRequestHandler<SurveyAssignmentCommand, GenericResponse<SurveyAssignmentResponse>>
{
    private readonly IApplicationDbContext _context;

    public SurveyAssignmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<SurveyAssignmentResponse>> Handle(SurveyAssignmentCommand request, CancellationToken cancellationToken)
    {
        var check = _context.UserSurveys.FirstOrDefault(x => x.UserId == request.UserId && x.SurveyItemId == request.Model.SurveyItemId);
        if(check != null)
            return GenericResponse<SurveyAssignmentResponse>.Success("Anket daha önce atanmıştır", 200);

        _context.UserSurveys.Add(new UserSurvey
        {
            UserSurveyId = request.InstanceId, // Guid.NewGuid().ToString(),
            SurveyItemId = request.SurveyItemId,
            UserId = request.UserId,
            Status = SurveyStatus.Appointed,
            Created = DateTime.Now,
            IsActived = 1,
            InstanceId = request.InstanceId, 
        });

        _context.SaveChanges();
        return GenericResponse<SurveyAssignmentResponse>.Success("", 200);
    }
}
