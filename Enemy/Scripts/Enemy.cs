using Godot;
using System;

public partial class Enemy : Node, IDamageable
{
	[Export]
	public bool isActive = false;

	[Export]
	public float speed = 3f;

	[Export]
	public bool isDead = false;

	[Export]
	public int damage = 10;

	[Export]
	public int life = 100;

	[Export]
	public CharacterBody3D enemyCharacter;

	private Node3D target;

	private AnimationPlayer animationPlayer;

	private CollisionShape3D collisionShape3D;

	// Called when the node enters the scene tree for the first time.p
	public override void _Ready()
	{
		if (enemyCharacter == null)
		{
			throw new Exception("You must assign a enemy character");
		}

		target = GetNodeOrNull<Node3D>("%Player");

		if (target == null)
		{

			GD.PushWarning("No target node with %Player found!");
		}

		animationPlayer = GetNode<AnimationPlayer>("CharacterBody3D/Origin/ShakeWhenMove");
		collisionShape3D = GetNode<CollisionShape3D>("CharacterBody3D/CollisionShape3D");

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

		if (target != null)
		{
			var targetRotation = target.Rotation;
			enemyCharacter.Rotation = targetRotation;
		}


		enemyCharacter.MoveAndSlide();

		animationPlayer.Play("Shake", -1, velocity.Length() / 2);

		CheckCollision();

	}

	public void TakeDamage(int amount)
	{
		if (life >= amount)
		{
			life -= amount;
		}
		else
		{
			life = 0;
		}
	}


	public void CheckCollision()
	{
		var collisionCount = enemyCharacter.GetSlideCollisionCount();
		if (collisionCount > 0)
		{

			for (int i = 0; i < collisionCount; i++)
			{
				var collision = enemyCharacter.GetSlideCollision(i);

				var node = collision.GetCollider();

				if (node != null && node is Player)
				{
					var player = (Player)node;

					player.TakeDamage(damage);
				}

			}

		}
	}
}
