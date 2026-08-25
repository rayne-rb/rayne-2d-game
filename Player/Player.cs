using System;
using dgameincsharp.Components;
using dgameincsharp.GameCore.Utility;
using Godot;
using Timer = Godot.Timer;

namespace dgameincsharp.Player;

public partial class Player : CharacterBody2D
{
    [ExportGroup("Movement")] [Export] public float Speed = 45f;
    [Export] public float Acceleration = 16f;
    [Export] public float Deceleration = 18f;
    [Export] public float JumpDeceleration = 12f;
    [Export] public float JumpImpulse = 20f;

    [ExportGroup("Physics")] [Export] public float Gravity = -300f;
    [Export] public float MaxVelocity = 300f;

    [ExportGroup("General")] [Export] public AnimatedSprite2D PlayerSprite;
    [Export] public Timer JumpTimer;
    [Export] public MeleeAttackComponent MeleeAttackComponent;
    [Export] public AudioStreamPlayer2D PlayerSound;

    public event EventHandler<string> PlayerDirectionChanged;
    public string PlayerDirection = "right";

    public bool IsGrounded = false;
    public bool WasGrounded = false;
    public bool IsJumping = false;
    public bool PriorityAnimationPlaying = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (MeleeAttackComponent != null)
        {
            MeleeAttackComponent.EntityAttacking += PlayAttackAnimation;
            PlayerSprite.AnimationFinished += ReturnToNormalAnimation;
        }
        else
        {
            Loggy.Warning("Attack Component null!");
        }

        if (PlayerSound == null)
        {
            Loggy.Warning("Player Sound null!");
        }

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

            if (velocity.Y < -MaxVelocity)
                velocity.Y = -MaxVelocity;
        }

        if (Input.IsActionPressed("move_right"))
        {
            if (!PriorityAnimationPlaying)
            {
                PlayerSprite.Animation = "Walk";
            }

            velocity.X = Mathf.Lerp(velocity.X, Speed, Acceleration * (float)delta);
            if (PlayerSprite != null)
            {
                PlayerSprite.FlipH = false;
            }

            if (PlayerDirection != "right")
            {
                PlayerDirection = "right";

                if (PlayerDirectionChanged != null)
                {
                    PlayerDirectionChanged.Invoke(this, "right");
                }
            }
        }
        else if (Input.IsActionPressed("move_left"))
        {
            if (!PriorityAnimationPlaying)
            {
                PlayerSprite.Animation = "Walk";
            }

            velocity.X = Mathf.Lerp(velocity.X, -Speed, Acceleration * (float)delta);
            if (PlayerSprite != null)
            {
                PlayerSprite.FlipH = true;
            }

            if (PlayerDirection != "left")
            {
                PlayerDirection = "left";

                if (PlayerDirectionChanged != null)
                {
                    PlayerDirectionChanged.Invoke(this, "left");
                }
            }
        }
        else
        {
            if (!PriorityAnimationPlaying)
            {
                PlayerSprite.Animation = "Idle";
            }

            velocity.X = Mathf.Lerp(velocity.X, 0, Deceleration * (float)delta);
        }

        if (Input.IsActionPressed("look_down"))
        {
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    public void ReturnToNormalAnimation()
    {
        Loggy.Debug("Returning to normal animation!");
        PriorityAnimationPlaying = false;
        MeleeAttackComponent.IsAttacking = false;
        PlayerSprite.Play();
    }

    public void PlayAttackAnimation(object? sender, EventArgs e)
    {
        var random = new RandomNumberGenerator();
        var randomNumber = random.RandiRange(0, 1);
        
        if (PlayerSound != null)
        {
            var stream = new AudioStream();
            if (randomNumber == 0)
            {
                stream = GD.Load<AudioStream>("res://Player/Chomp.mp3");
            }
            else
            {
                stream = GD.Load<AudioStream>("res://Player/Chomp2.mp3");
            }
            PlayerSound.SetStream(stream);
            PlayerSound.Play();
        }

        PriorityAnimationPlaying = true;
        if (randomNumber == 0)
        {
            Loggy.Debug("Playing Bite1!");
            PlayerSprite.Animation = "Bite1";
        }
        else
        {
            Loggy.Debug("Playing Bite2!");
            PlayerSprite.Animation = "Bite2";
        }
    }
}