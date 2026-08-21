using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.Components;

public partial class MeleeAttackComponent : AttackComponent
{
    [ExportGroup("Melee Attack")]
    [Export] public Area2D AttackArea;
    
    public override void _Ready()
    {
        if (AttackArea == null)
        {
            Loggy.Error("Attack Area not set!");
            QueueFree();
        }
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("attack"))
        {
            Attack();
        }

        base._UnhandledInput(@event);
    }
    
    public void Attack()
    {
        Loggy.Debug("Attack input received.");
        if (AttackArea.HasOverlappingBodies())
        {
            // Loggy.Debug("Attacking!");
            var bodies = AttackArea.GetOverlappingBodies();
            foreach (var body in bodies)
            {
                foreach (var child in body.GetChildren())
                {
                    if (child is HealthComponent healthComponent)
                    {
                        DealDamage(healthComponent);
                    }
                }
            }
        }
    }
}