using Microsoft.EntityFrameworkCore;
using Oobjs.Lesson1.Core.Data;
using Oobjs.Lesson1.Core.Entities;

namespace Oobjs.Lesson1.Core.Services;

public class OobjService
{
    private readonly OobjsDbContext _db;

    public OobjService(OobjsDbContext db)
    {
        _db = db;
    }

    public async Task<List<Oobj>> GetAllAsync()
    {
        return await _db.Oobjs
            .AsNoTracking()
            .OrderBy(x => x.DisplayName)
            .ToListAsync();
    }

    public async Task<Oobj?> GetAsync(Guid guid)
    {
        return await _db.Oobjs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Guid == guid);
    }

    public async Task<Oobj> CreateAsync(
        string displayName,
        string objectTypeName)
    {
        var oobj = new Oobj
        {
            Guid = Guid.NewGuid(),
            DisplayName = displayName,
            ObjectTypeName = objectTypeName
        };

        _db.Oobjs.Add(oobj);
        await _db.SaveChangesAsync();

        return oobj;
    }

    public async Task<bool> UpdateAsync(
        Guid guid,
        string displayName,
        string objectTypeName)
    {
        var oobj = await _db.Oobjs
            .FirstOrDefaultAsync(x => x.Guid == guid);

        if (oobj is null)
            return false;

        oobj.DisplayName = displayName;
        oobj.ObjectTypeName = objectTypeName;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteOobjAsync(Guid guid)
    {
        var oobj = await _db.Oobjs
            .FirstOrDefaultAsync(x => x.Guid == guid);

        if (oobj is null)
            return false;

        //titta om det finns relations kasta exception
        var existingRelation = await
        _db.Relations.FirstOrDefaultAsync(x => x.ChildGuid == guid
        || x.ParentGuid == guid);
        if(existingRelation is not null)
        {
            return false;
        }

        _db.Oobjs.Remove(oobj);
        await _db.SaveChangesAsync();

        return true;
    }




    //steg 31 utökning
    public async Task<List<FieldValue>> GetFieldsAsync(Guid oobjGuid)
    {
        return await _db.FieldValues
            .AsNoTracking()
            .Where(x => x.OobjGuid == oobjGuid)
            .OrderBy(x => x.Field)
            .ToListAsync();
    }

    public async Task<FieldValue> AddFieldAsync(
        Guid oobjGuid,
        string field,
        string value)
    {
        var oobjExists = await _db.Oobjs
            .AnyAsync(x => x.Guid == oobjGuid);

        if (!oobjExists)
            throw new InvalidOperationException("Oobj does not exist.");
        if(field == "string")
        {
            throw new InvalidOperationException("Field cant be named string");
        }

        var fieldValue = new FieldValue
        {
            OobjGuid = oobjGuid,
            Field = field,
            Value = value
        };

        _db.FieldValues.Add(fieldValue);
        await _db.SaveChangesAsync();

        return fieldValue;
    }

    public async Task<bool> UpdateFieldAsync(
       Guid oobjGuid,
       string field,
       string value)
    {
        var oobjExists = await _db.Oobjs
            .AnyAsync(x => x.Guid == oobjGuid);

        if (!oobjExists)
            throw new InvalidOperationException("Oobj does not exist.");
        var existingFieldValue = await _db.FieldValues
            .FirstOrDefaultAsync(x => x.OobjGuid == oobjGuid && x.Field == field);
        if (existingFieldValue == null)
            return false;

        existingFieldValue.Value = value;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveFieldAsync(
       Guid oobjGuid, string fieldName)
    {
        var oobjExists = await _db.Oobjs
            .FirstOrDefaultAsync(x => x.Guid == oobjGuid);

        if (oobjExists == null)
            throw new InvalidOperationException("Oobj does not exist.");

        //could use ordinal case
        var fieldExists = await _db.FieldValues.Where(g => g.Field == fieldName)   
            .FirstOrDefaultAsync(x => x.OobjGuid == oobjExists.Guid);

        if (fieldExists == null)
            throw new InvalidOperationException("Field does not exist.");

        _db.FieldValues.Remove(fieldExists);

        await _db.SaveChangesAsync();

        return true;
    }



    //steg 33

    public async Task<Relation> CreateRelationAsync(
    Guid parentGuid,
    Guid childGuid)
    {
        var parentExists = await _db.Oobjs
            .FirstOrDefaultAsync(x => x.Guid == parentGuid);

        var childExists = await _db.Oobjs
            .FirstOrDefaultAsync(x => x.Guid == childGuid);

        if (parentExists is not null || childExists is not null)
            return null;
        if (parentExists.Guid == childExists.Guid)
            return null;


        var relation = new Relation
        {
            ParentGuid = parentGuid,
            ChildGuid = childGuid
        };

        _db.Relations.Add(relation);
        await _db.SaveChangesAsync();

        return relation;
    }

    public async Task<bool> DeleteRelationsAsync(Guid parentGuid,Guid childGuid)
    {
        var parentExists = await _db.Oobjs
            .AnyAsync(x => x.Guid == parentGuid);

        var childExists = await _db.Oobjs
            .AnyAsync(x => x.Guid == childGuid);

        if (!parentExists || !childExists)
            throw new InvalidOperationException("Parent or child Oobj does not exist.");

        var relation = await _db.Relations.FirstOrDefaultAsync
            (x => x.ParentGuid == parentGuid && x.ChildGuid == childGuid);
        if (relation == null)
            return false;

        _db.Relations.Remove(relation);
        await _db.SaveChangesAsync();

        return true;
    }


    public async Task<List<Relation>> GetChildRelationsAsync(
        Guid parentGuid)
    {
        return await _db.Relations
            .AsNoTracking()
            .Where(x => x.ParentGuid == parentGuid)
            .ToListAsync();
    }

    public async Task<List<string>> GetAllUniqueOobjTypeNames()
    {
        return await _db.Oobjs
        .AsNoTracking()
        .Select(x => x.ObjectTypeName)
        .Distinct()
        .OrderBy(x => x)
        .ToListAsync();
    }

    public async Task<List<Oobj>> GetOobjTypeName(string? type)
    {
        var query = _db.Oobjs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(type)) //search specific typeName
        {
            query = query.Where(x => x.ObjectTypeName == type);
        }

        return await query.ToListAsync();
    }

    public async Task<List<Oobj>> GetSearchQueryRelation(string? query)
    {
        return await _db.Oobjs
        .AsNoTracking()
        .Where(x => EF.Functions.ILike(x.DisplayName, $"%{query}%"))
        .Take(50)
        .OrderBy(x => x.DisplayName)
        .ToListAsync();
    }

}