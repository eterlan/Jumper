using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public class DebugMessageComponent : IComponent
{
    public string message;
}