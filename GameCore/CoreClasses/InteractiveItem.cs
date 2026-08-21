using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.GameCore.CoreClasses;

[GlobalClass]
public partial class InteractiveItem : Node2D
{
    [Export] public string ItemName;
    [Export] public string ItemDescription;
    [Export] public Area2D InteractiveArea;
    
    public override void _Ready()
    {
        if (InteractiveArea == null)
        {
            Loggy.Error("Interactive Area not set!");
            QueueFree();
        }
    }
    
    public virtual void _OnInteractiveAreaAreaEntered(Area2D area)
    {
        
    }
    
}