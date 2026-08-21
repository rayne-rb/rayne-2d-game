using Godot;

namespace dgameincsharp.GameCore.CoreClasses;

[GlobalClass]
public partial class Effect : Node
{
    [Export] public string EffectName;
    [Export] public string EffectDescription;
    [Export] public bool IsInstant;
    [Export] public float Duration;

    public override void _Ready()
    {
    }
    
    public override void _Process(double delta)
    {
    }
    
    public void ApplyEffect()
    {
    }
}