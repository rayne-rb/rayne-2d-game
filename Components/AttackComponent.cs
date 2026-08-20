using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.Components;

[GlobalClass]
public partial class AttackComponent : Node2D
{
    [Export] public float BaseDamage = 10;
    [Export] public float CritChance = 0.2f;
    [Export] public float CritMultiplier = 2;

    public Area2D AttackArea;

    public override void _Ready()
    {
        var hasAttackArea = false;
        var children = GetChildren();
        Loggy.Debug($"Found {children.Count} children.");
        if (children.Count > 0)
        {
            foreach (var child in children)
            {
                Loggy.Debug($"Checking child {child.Name}");
                if (child is Area2D area2D)
                {
                    AttackArea = area2D;
                    hasAttackArea = true;
                    Loggy.Debug("Attack Area set!");
                    break;
                }
            }
        }

        if (!hasAttackArea)
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

    private void Attack()
    {
        Loggy.Debug("Attack input received.");
        if (AttackArea.HasOverlappingBodies())
        {
            Loggy.Debug("Attacking!");
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

    public void DealDamage(HealthComponent target)
    {
        Loggy.Debug("Dealing Damage!");
        var damage = BaseDamage;
        if (GD.Randf() < CritChance)
            damage *= CritMultiplier;

        target.TakeDamage((int)damage);
        Loggy.Debug($"Dealt {damage} damage to {target.Name}");
    }
}