using System;
using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.Components;

[GlobalClass]
public partial class MeleeAttackComponent : AttackComponent
{
    [ExportGroup("Melee Attack")]
    [Export] public Area2D AttackArea;
    [Export] public Player.Player PlayerBody;
    
    public override void _Ready()
    {
        if (PlayerBody != null)
        {
            PlayerBody.PlayerDirectionChanged += ChangeAttackDirection;
        }
        else
        {
            Loggy.Warning("PlayerBody null!");
        }
        
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

    public void ChangeAttackDirection(object sender, string direction)
    {
        var transform = AttackArea.Transform;
        transform.X = -transform.X;
        
        AttackArea.Transform  = transform;
        Loggy.Debug($"Transform.X {transform.X} | Transform.Y {transform.Y} | Direction {direction}");
    }
    
}