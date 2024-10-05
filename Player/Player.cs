using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float FireSpeedFactor = 0.5f;
	public const float JumpVelocity = 4.5f;
	private readonly float RotateSpeed = Mathf.DegToRad(-90.0f);
	[Export]
	public Node Weapon; // IWeapon

	public override void _PhysicsProcess(double delta)
	{
		var isFiring = Input.IsActionPressed("fire");
		if (Weapon != null)
		{
			(Weapon as IWeapon).Emitting = isFiring;
		}

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		var speedFactor = isFiring ? FireSpeedFactor : 1f;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(0, 0, inputDir.Y)).Normalized();
		float rotation = inputDir.X * RotateSpeed;
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed * speedFactor;
			velocity.Z = direction.Z * Speed * speedFactor;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		if (rotation != 0)
		{
			Transform = Transform.RotatedLocal(Vector3.Up, (float)(rotation * delta * speedFactor));
		}
		MoveAndSlide();
	}
}
