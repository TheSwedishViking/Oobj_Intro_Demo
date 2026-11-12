using Microsoft.AspNetCore.Mvc;
using Oobjs.Lesson1.Api.Models;
using Oobjs.Lesson1.Core.Services;

namespace Oobjs.Lesson1.Api.Controllers;

[ApiController]
[Route("api/oobjs")]
public class OobjsController : ControllerBase
{
    private readonly OobjService _service;

    public OobjsController(OobjService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{guid:guid}")]
    public async Task<IActionResult> Get(Guid guid)
    {
        var result = await _service.GetAsync(guid);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOobjRequest request)
    {
        var result = await _service.CreateAsync(
            request.DisplayName,
            request.ObjectTypeName);

        return Ok(result);
    }

    [HttpPut("{guid:guid}")]
    public async Task<IActionResult> Update(
        Guid guid,
        UpdateOobjRequest request)
    {
        var success = await _service.UpdateAsync(
            guid,
            request.DisplayName,
            request.ObjectTypeName);

        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{guid:guid}")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var success = await _service.DeleteAsync(guid);

        if (!success)
            return NotFound();

        return NoContent();
    }



    //steg 31 

    [HttpGet("{guid:guid}/fields")]
    public async Task<IActionResult> GetFields(Guid guid)
    {
        var oobj = await _service.GetAsync(guid);

        if (oobj is null)
            return NotFound();

        var result = await _service.GetFieldsAsync(guid);
        return Ok(result);
    }

    [HttpPost("{guid:guid}/fields")]
    public async Task<IActionResult> AddField(
        Guid guid,
        CreateFieldValueRequest request)
    {
        var oobj = await _service.GetAsync(guid);

        if (oobj is null)
            return NotFound();

        var result = await _service.AddFieldAsync(
            guid,
            request.Field,
            request.Value);

        return Ok(result);
    }

}