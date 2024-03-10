using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nScheduler.Domain.Repositories.Configs;
using nScheduler.Domain.Repositories.Jobs;
using nScheduler.Domain.ViewModels.Accounts;

namespace nScheduler.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class IndexController : Controller
{
    private readonly IParameterCfgRepository parameter;
    private readonly IImageCfgRepository image;
    private readonly IMessageCfgRepository message;
    private readonly IUserRepository user;
    private readonly IJobInfoRepository jobInfo;
    private readonly IJobLogRepository jobLog;

    public IndexController(IParameterCfgRepository parameter, IImageCfgRepository image,
        IMessageCfgRepository message, IUserRepository user,
        IJobInfoRepository jobInfo, IJobLogRepository jobLog)
    {
        this.parameter = parameter;
        this.image = image;
        this.message = message;
        this.user = user;
        this.jobInfo = jobInfo;
        this.jobLog = jobLog;
    }

    [HttpGet]
    public async Task<IndexViewModel> Get(CancellationToken cancellationToken = default)
    {
        var joblogCount = await jobLog.GetJobLogGroupByDay(x => x.BeginTime > DateTime.Now.AddDays(-30));
        return new IndexViewModel
        {
            JobAllCount = await jobInfo.Count(null, cancellationToken),
            JobCanRunCount = await jobInfo.Count(x => x.Status != -1, cancellationToken),
            ParamCount = await parameter.Count(null, cancellationToken),
            ImageCount = await image.Count(null, cancellationToken),
            MessageCount = await message.Count(null, cancellationToken),
            UserCount = await user.Count(null, cancellationToken),
            JobLogGroupDay = Enumerable.Range(0, 30).Select(x => DateTime.Now.AddDays(x - 30).ToString("MMdd")).ToDictionary(x => x, x => joblogCount.TryGetValue(x, out var count) ? count : 0),
            JobTypeGroupDay = await jobInfo.GetJobTypeGroupByDay(cancellationToken)
        };
    }
}