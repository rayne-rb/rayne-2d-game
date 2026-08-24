using dgameincsharp.GameCore.Utility;
using Godot;

namespace dgameincsharp.Components;

[GlobalClass]
public partial class HealthComponent : Node
{
	[Export] public float Health = 100;
	[Export] public float MaxHealth = 100;
	[Export] public bool IsFriendly = false;

	[Signal]
	delegate void DeathEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeDamage(float damage, bool fromFriend = false)
	{
		if (fromFriend && !IsFriendly || !fromFriend && IsFriendly)
		{
			Health -= damage;
			if (Health <= 0)
			{
				Health = 0;
				// EmitSignal(nameof(DeathEventHandler));
				GetParent().QueueFree();
			}

			Loggy.Debug($"Took {damage} damage, {Health} health remaining");
		}
	}

	public void Heal(float heal)
	{
		Health += heal;
		if (Health > MaxHealth)
			Health = MaxHealth;

		Loggy.Debug($"Healed {heal} health, {Health} health remaining");
	}
}
