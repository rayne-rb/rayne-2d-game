using Godot;
using System;
using dgameincsharp.GameCore.Utility;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Movement")] [Export] public float Speed = 45f;
	[Export] public float Acceleration = 16f;
	[Export] public float Deceleration = 18f;
	[Export] public float JumpImpulse = 20f;

	[ExportGroup("Physics")] [Export] public float Gravity = -20f;

	[ExportGroup("General")] [Export] public Sprite2D PlayerSprite;

	public bool IsGrounded = false;
	public bool IsJumping = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Loggy.Debug("Initializing Player...");
		Loggy.Info("Initializing Player...");
		Loggy.Warning("This is a warning!");
		Loggy.Error("This is an error!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		IsGrounded = IsOnFloor();

		if (IsGrounded)
		{
			if (Input.IsActionJustPressed("jump"))
			{
				velocity.Y = -JumpImpulse;
				IsJumping = true;
			}
			else
			{
				IsJumping = false;
			}
		}
		else
		{
			if (IsJumping)
			{
				if (velocity.Y < 0.0 && !Input.IsActionPressed("jump"))
				{
					// velocity.Y -= Gravity * JumpImpulse * (float)delta;
					velocity.Y = 0;
				}
				else
				{
					velocity.Y -= Gravity * (float)delta;
				}
			}
			else
			{
				velocity.Y -= Gravity * (float)delta;
			}
		}

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X = Mathf.Lerp(velocity.X, Speed, Acceleration * (float)delta);
			if (PlayerSprite != null)
			{
				PlayerSprite.FlipH = false;
			}
		}
		else if (Input.IsActionPressed("move_left"))
		{
			velocity.X = Mathf.Lerp(velocity.X, -Speed, Acceleration * (float)delta);
			if (PlayerSprite != null)
			{
				PlayerSprite.FlipH = true;
			}
		}
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, 0, Deceleration * (float)delta);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
