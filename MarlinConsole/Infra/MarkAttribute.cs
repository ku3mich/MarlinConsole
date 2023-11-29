namespace MarlinConsole.Infra;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class MarkAttribute(params object[] marks) : Attribute
{
    public object[] Marks { get; set; } = marks;
}

