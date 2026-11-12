namespace Oobjs.Lesson1.Api.Models;

public class CreateRelationRequest
{
    public Guid ParentGuid { get; set; }

    public Guid ChildGuid { get; set; }
}