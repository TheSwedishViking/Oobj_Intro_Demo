namespace Oobjs.Lesson1.Api.Models;

public class CreateFieldValueRequest
{
    public string Field { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}