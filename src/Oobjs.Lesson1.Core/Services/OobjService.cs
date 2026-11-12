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

    public async Task<bool> DeleteAsync(Guid guid)
    {
        var oobj = await _db.Oobjs
            .FirstOrDefaultAsync(x => x.Guid == guid);

        if (oobj is null)
            return false;

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


    //steg 33

    public async Task<Relation> CreateRelationAsync(
    Guid parentGuid,
    Guid childGuid)
    {
        var parentExists = await _db.Oobjs
            .AnyAsync(x => x.Guid == parentGuid);

        var childExists = await _db.Oobjs
            .AnyAsync(x => x.Guid == childGuid);

        if (!parentExists || !childExists)
            throw new InvalidOperationException("Parent or child Oobj does not exist.");

        var relation = new Relation
        {
            ParentGuid = parentGuid,
            ChildGuid = childGuid
        };

        _db.Relations.Add(relation);
        await _db.SaveChangesAsync();

        return relation;
    }

    public async Task<List<Relation>> GetChildRelationsAsync(
        Guid parentGuid)
    {
        return await _db.Relations
            .AsNoTracking()
            .Where(x => x.ParentGuid == parentGuid)
            .ToListAsync();
    }

}