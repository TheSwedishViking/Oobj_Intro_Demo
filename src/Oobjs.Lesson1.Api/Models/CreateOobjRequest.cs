namespace Oobjs.Lesson1.Api.Models;

public class CreateOobjRequest
{
    public string DisplayName { get; set; } = string.Empty;

    public string ObjectTypeName { get; set; } = string.Empty;
}