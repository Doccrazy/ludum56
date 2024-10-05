using Godot;
using System;

public partial class Enemy : Node
{
	[Export]
	public bool isActive = false;

	[Export]
	public float speed = 3f;

	[Export]
	public bool isDead = false;

	[Export]
	public float damage = 10f;

	[Export]
	public CharacterBody3D enemyCharacter;

	private Node3D target;

	private AnimationPlayer animationPlayer;

	private bool isMoving = false;

	// Called when the node enters the scene tree for the first time.p
	public override void _Ready()
	{
		if (enemyCharacter == null)
		{
			throw new Exception("You must assign a enemy character");
		}

		target = GetNode<Node3D>("%Player");

		GD.Print("TARGET NODE: ", target);

		animationPlayer = GetNode<AnimationPlayer>("CharacterBody3D/Origin/ShakeWhenMove");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = enemyCharacter.Velocity;
		// Give the enemy char some gravity
		if (!enemyCharacter.IsOnFloor())
		{
			velocity += enemyCharacter.GetGravity() * (float)delta;
		}

		// If an target exists then flow this target
		if (target != null && !isDead)
		{
			var targetPos = target.GlobalPosition;
			var enemyPos = enemyCharacter.GlobalPosition;


			var moveVector = enemyPos.DirectionTo(targetPos);
			var moveVelocity = moveVector * speed;

			velocity.X = moveVelocity.X;
			velocity.Z = moveVelocity.Z;
		}

		enemyCharacter.Velocity = velocity;
		isMoving = enemyCharacter.MoveAndSlide();

		var targetRotation = target.Rotation;
		enemyCharacter.Rotation = targetRotation;

		animationPlayer.Play("Shake", -1, enemyCharacter.Velocity.Length() / 2);

	}
}
