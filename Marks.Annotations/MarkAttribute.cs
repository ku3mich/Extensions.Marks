namespace Marks.Annotations;

[AttributeUsage(
    AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property,
    AllowMultiple = true, Inherited = false)]
public class MarkAttribute(params object[] marks) : Attribute
{
    public object[] Marks { get; set; } = marks;
}

