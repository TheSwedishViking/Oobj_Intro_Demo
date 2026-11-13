using Microsoft.AspNetCore.Mvc;
using Oobjs.Lesson1.Api.Models;
using Oobjs.Lesson1.Core.Entities;
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
        var oobjExists= await _service.GetAsync(guid);
        if(oobjExists is null)
            return NotFound();

        var success = await _service.DeleteOobjAsync(guid);

        if (!success)
            return Conflict();

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

    [HttpDelete("{guid:guid}/fields")]
    public async Task<IActionResult> RemoveField(Guid guid, string fieldName)
    {
        var oobj = await _service.GetAsync(guid);
        if (oobj is null)
            return NotFound();


        var success = await _service.RemoveFieldAsync(guid, fieldName);

        if (!success)
            return NotFound();

        return NoContent();
    }

     

    [HttpPut("{guid:guid}/fields")]
    public async Task<IActionResult> AddOrUpdateField(Guid guid
        ,CreateFieldValueRequest request)
    {
        var oobj = await _service.GetAsync(guid);
        if (oobj is null)  return NotFound();


        //Uppdatera FieldValue
        bool existingFieldValue = await _service.UpdateFieldAsync(guid,
            request.Field, request.Value);

        //skapa ny FieldValue
        if (existingFieldValue == false)
        {
            var result = await _service.AddFieldAsync(guid,
            request.Field, request.Value);
            return Ok(result);

        }
        return NoContent();
    }

    //Lektion 2
    [HttpGet("types")]
    public async Task<IActionResult> GetTypes( string? type)
    {

        if(string.IsNullOrEmpty(type))
        {
            var result = await _service.GetAllUniqueOobjTypeNames();

            if (result is null)
                return NotFound();
            return Ok(result);
        }
        else
        {
            var result = await _service.GetOobjTypeName(type);
            return Ok(result);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchDisplayName(string? query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return Ok(new List<Oobj>());
        }

        var result = await _service.GetSearchQueryRelation(query);
        return Ok(result);
    }
}