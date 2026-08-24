using System;
using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.Components;

[GlobalClass]
public partial class MeleeAttackComponent : AttackComponent
{
    [ExportGroup("Melee Attack")] [Export] public Area2D AttackArea;
    [Export] public Player.Player PlayerBody;
    [Export] public Timer AnimationAttackDelayTimer;
    [Export] public bool AlwaysActive = false;
    public event EventHandler EntityAttacking;
    public bool IsAttacking = false;

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

        if (AnimationAttackDelayTimer != null)
        {
            AnimationAttackDelayTimer.Timeout += Attack;
        }
        else
        {
            Loggy.Warning("Animation Attack Timer not set!");
        }

        if (AttackArea == null)
        {
            Loggy.Error("Attack Area not set!");
            QueueFree();
        }
    }

    public override void _Process(double delta)
    {
        if (AlwaysActive)
        {
            if (!IsAttacking)
            {
                Attack();
            }
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (PlayerBody == null) return;

        if (@event.IsActionPressed("attack"))
        {
            if (!IsAttacking)
            {
                Loggy.Debug("Starting Attack Delay!");
                EntityAttacking?.Invoke(this, EventArgs.Empty);
                AnimationAttackDelayTimer.Start();
                // Attack();
                //Attack will happen when the timer times out.
            }
        }

        base._UnhandledInput(@event);
    }

    public void Attack()
    {
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
                        IsAttacking = true;
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

        AttackArea.Transform = transform;
        Loggy.Debug($"Transform.X {transform.X} | Transform.Y {transform.Y} | Direction {direction}");
    }
}