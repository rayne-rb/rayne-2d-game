using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.Components;

[GlobalClass]
public partial class AttackComponent : Node2D
{
    [ExportGroup("Attack Details")]
    [Export] public float BaseDamage = 10;
    [Export] public float CritChance = 0.2f;
    [Export] public float CritMultiplier = 2;
    [Export] public bool IsFromFriend = false;

    public void DealDamage(HealthComponent target)
    {
        // Loggy.Debug("Dealing Damage!");
        var damage = BaseDamage;
        if (GD.Randf() < CritChance)
            damage *= CritMultiplier;

        Loggy.Debug($"Is from friend: {IsFromFriend}");
        target.TakeDamage((int)damage, IsFromFriend);
        // Loggy.Debug($"Dealt {damage} damage to {target.Name}");
    }
}