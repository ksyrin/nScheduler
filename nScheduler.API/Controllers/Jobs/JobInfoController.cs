using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nScheduler.Common.Models;
using nScheduler.Domain.ViewModels.Jobs;
using nScheduler.Imp.Events.JobInfo;

namespace nScheduler.API.Controllers.Jobs;

/// <summary>
/// 作业列表
/// </summary>
[Route("api/[controller]/[Action]")]
[Authorize]
public class JobInfoController : Controller
{
    private readonly IMediator mediator;

    public JobInfoController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<JobViewModel?> Get([FromQuery] string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobInfoSignle { Id = id }, cancellationToken);
    }

    [HttpPost]
    public async Task<QueryResult<JobListViewModel>> Page([FromBody] JobInfoSearchViewModel viewModel, int page, int size, CancellationToken cancellationToken = default)
    {
        var model = new JobInfoPage
        {
            Model = viewModel,
            Page = page,
            Size = size
        };
        var response = await mediator.Send(model, cancellationToken);

        return new QueryResult<JobListViewModel>()
        {
            Total = response.Item1,
            Items = response.Item2
        };
    }

    [HttpPost]
    public async Task<BaseResult> Edit([FromQuery] string id, [FromBody] JobViewModel model, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobInfoEdit
        {
            Id = id,
            Model = model,
            UserName = HttpContext.User!.Identity!.Name!
        }, cancellationToken);
    }

    [HttpPost]
    public async Task<BaseResult> Delete([FromForm] string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobInfoDel
        {
            Id = id
        }, cancellationToken);
    }

    [HttpPost]
    public async Task<BaseResult> Exec([FromForm] string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobInfoExec
        {
            Id = id
        }, cancellationToken);
    }

    [HttpPost]
    public async Task<BaseResult> InValid([FromForm] string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new JobInfoInValid
        {
            Id = id
        }, cancellationToken);
    }
}