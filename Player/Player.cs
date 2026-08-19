using Godot;
using System;

public partial class Player : Node2D
{
	[Export]
	public float Speed = 12f;
	[Export] 
	public float Acceleration = 10f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("move_right"))
		{
			GD.Print("Right");
			this.Position += new Vector2(Speed * (float)delta, this.Position.Y);
		}
	}
}
