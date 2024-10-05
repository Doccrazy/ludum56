using Godot;

public partial class Enemy : Node, IDamageable
{
	[Export]
	public bool IsActive = false;

	[Export]
	public float Speed = 3f;

	[Export]
	public bool IsDead = false;

	[Export]
	public int Damage = 10;

	[Export]
	public int Life = 100;

	[Export]
	public CharacterBody3D EnemyCharacter;

	[Export]
	public int DamageCoolDownTimeInSec = 2;

	private Node3D _target;

	private AnimationPlayer _animationPlayer;

	private CollisionShape3D _collisionShape3D;

	private bool _coolDown = false;


	// Called when the node enters the scene tree for the first time.p
	public override void _Ready()
	{
		if (EnemyCharacter == null)
		{
			throw new System.Exception("You must assign a enemy character");
		}

		_target = GetNodeOrNull<Node3D>("%Player");

		if (_target == null)
		{

			GD.PushWarning("No target node with %Player found!");
		}

		_animationPlayer = GetNode<AnimationPlayer>("CharacterBody3D/Origin/ShakeWhenMove");
		_collisionShape3D = GetNode<CollisionShape3D>("CharacterBody3D/CollisionShape3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = EnemyCharacter.Velocity;
		// Give the enemy char some gravity
		if (!EnemyCharacter.IsOnFloor())
		{
			velocity += EnemyCharacter.GetGravity() * (float)delta;
		}

		// If an target exists then flow this target
		if (_target != null && !IsDead)
		{
			var targetPos = _target.GlobalPosition;
			var enemyPos = EnemyCharacter.GlobalPosition;


			var moveVector = enemyPos.DirectionTo(targetPos);
			var moveVelocity = moveVector * Speed;

			velocity.X = moveVelocity.X;
			velocity.Z = moveVelocity.Z;
		}

		EnemyCharacter.Velocity = velocity;

		if (_target != null)
		{
			var targetRotation = _target.Rotation;
			EnemyCharacter.Rotation = targetRotation;
		}

		EnemyCharacter.MoveAndSlide();

		_animationPlayer.Play("Shake", -1, velocity.Length() / 2);

		CheckCollision();

	}

	public void TakeDamage(int amount)
	{
		if (Life >= amount)
		{
			Life -= amount;
		}
		else
		{
			Life = 0;
			QueueFree();
		}
	}

	public void CheckCollision()
	{
		var collisionCount = EnemyCharacter.GetSlideCollisionCount();
		if (collisionCount > 0)
		{

			for (int i = 0; i < collisionCount; i++)
			{
				var collision = EnemyCharacter.GetSlideCollision(i);

				var node = collision.GetCollider();

				if (node != null && node is Player)
				{
					var player = (Player)node;
					if (!_coolDown)
					{
						_coolDown = true;
						player.TakeDamage(Damage);
						DisableCoolDownAsync();
					}
				}
			}
		}
	}

	public async void DisableCoolDownAsync()
	{
		Timer timer = new Timer();

		AddChild(timer);
		timer.WaitTime = DamageCoolDownTimeInSec;

		timer.OneShot = true;

		timer.Start();

		await ToSignal(timer, "timeout");

		_coolDown = false;
		timer.QueueFree();

	}
}
