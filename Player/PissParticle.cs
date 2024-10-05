using Godot;
using System;

public partial class PissParticle : RigidBody3D
{
	public Node3D TrailTarget;
	[Export]
	public Node3D Trail;
	[Export]
	public PackedScene DecalScene;
	[Export]
	public int Damage = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Trail != null)
		{
			var scale = Trail.Scale;
			scale.Z = 0f;
			Trail.Scale = scale;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Trail != null && TrailTarget != null && IsInstanceValid(TrailTarget))
		{
			var dist = GlobalPosition - TrailTarget.GlobalPosition;
			var scale = Trail.Scale;
			scale.Z = dist.Length();
			if (scale.Z > 0)
			{
				Trail.GlobalTransform = GlobalTransform.LookingAt(TrailTarget.GlobalPosition, Perpendicular(dist));
				Trail.Scale = scale;
			}
		}
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}

	public void OnBodyEntered(Node body)
	{
		if (body is StaticBody3D)
		{
			var collision = new KinematicCollision3D();
			if (TestMove(GlobalTransform, Vector3.Zero, collision, 0.001f, true))
			{
				var decal = DecalScene.Instantiate<Node3D>();
				body.AddChild(decal);
				decal.GlobalPosition = collision.GetPosition();
				decal.GlobalRotation = collision.GetNormal();
				decal.RotateY(GD.Randf() * Mathf.Pi);
			}
		}
		else if (body is IDamageable)
		{
			(body as IDamageable).TakeDamage(Damage);
		}
		else if (body.GetParent() is IDamageable)
		{
			body.GetParent<IDamageable>().TakeDamage(Damage);
		}
		QueueFree();
	}

	private static Vector3 Perpendicular(Vector3 orig)
	{
		return orig.Z < orig.X ? new Vector3(orig.Y, -orig.X, 0) : new Vector3(0, -orig.Z, orig.Y);
	}
}
