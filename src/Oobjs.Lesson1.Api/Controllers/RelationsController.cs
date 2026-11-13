using Microsoft.AspNetCore.Mvc;
using Oobjs.Lesson1.Api.Models;
using Oobjs.Lesson1.Core.Services;

namespace Oobjs.Lesson1.Api.Controllers;

[ApiController]
[Route("api/relations")]
public class RelationsController : ControllerBase
{
    private readonly OobjService _service;

    public RelationsController(OobjService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRelationRequest request)
    {
        var parent = await _service.GetAsync(request.ParentGuid);
        var child = await _service.GetAsync(request.ChildGuid);

        if (parent is null || child is null)
            return BadRequest("ParentGuid and ChildGuid must reference existing Oobjs.");

        var result = await _service.CreateRelationAsync(
            request.ParentGuid,
            request.ChildGuid);
        if (result is null)
            return Conflict("Relation already exists or invalid.");

        return Ok(result);
    }

    [HttpGet("parent/{parentGuid:guid}")]
    public async Task<IActionResult> GetChildren(Guid parentGuid)
    {
        var result =
            await _service.GetChildRelationsAsync(parentGuid);

        return Ok(result);
    }

    [HttpDelete("parent/{parentGuid:guid}/child/{childGuid:guid}")]
    public async Task<IActionResult> Delete(Guid parentGuid, Guid childGuid)
    {
        var deleteRelation = await _service.
        DeleteRelationsAsync(parentGuid, childGuid);
        if (deleteRelation == false)
            return Conflict();

        return Ok();

    }
}