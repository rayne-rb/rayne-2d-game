using Godot;
using System;
using System.Threading;
using dgameincsharp.GameCore.Utility;
using Timer = Godot.Timer;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Movement")] 
	[Export] public float Speed = 45f;
	[Export] public float Acceleration = 16f;
	[Export] public float Deceleration = 18f;
	[Export] public float JumpDeceleration = 12f;
	[Export] public float JumpImpulse = 20f;

	[ExportGroup("Physics")] 
	[Export] public float Gravity = -300f;
	[Export] public float MaxVelocity = 300f;

	[ExportGroup("General")] 
	[Export] public Sprite2D PlayerSprite;
	[Export] public Timer JumpTimer;

	public bool IsGrounded = false;
	public bool WasGrounded = false;
	public bool IsJumping = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		JumpTimer.OneShot = true;
		Loggy.Info("Initializing Player...");
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
				WasGrounded = true;
			}
		}
		else
		{
			if (WasGrounded)
			{
				// Loggy.Debug("Jump timer started!");
				JumpTimer.Start();
				WasGrounded = false;
			}

			if (!JumpTimer.IsStopped())
			{
				// Loggy.Debug($"Jump Timer Time: {JumpTimer.TimeLeft}");
				if (Input.IsActionJustPressed("jump"))
				{
					velocity.Y = -JumpImpulse;
					IsJumping = true;
					WasGrounded = false;
				}
			}
			else
			{
				// Loggy.Debug("Jump timer stopped!");
			}
			
			if (IsJumping)
			{
				if (velocity.Y < 0.0 && !Input.IsActionPressed("jump"))
				{
					velocity.Y -= Gravity * JumpDeceleration * (float)delta;
					// velocity.Y = 0;
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
			
			if(velocity.Y < -MaxVelocity)
				velocity.Y = -MaxVelocity;
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
