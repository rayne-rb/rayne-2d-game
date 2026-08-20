using Godot;

namespace dgameincsharp.Components;

[GlobalClass]
public partial class HealthComponent : Node
{
	[Export]
	public int Health = 100;
	[Export]
	public int MaxHealth = 100;
	
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
	
	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Health = 0;
			EmitSignal(nameof(DeathEventHandler));
		}
	}

	public void Heal(int heal)
	{
		Health += heal;
		if (Health > MaxHealth)
			Health = MaxHealth;
	}
}
