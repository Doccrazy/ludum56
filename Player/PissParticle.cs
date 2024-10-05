using Godot;
using System;

public partial class PissParticle : RigidBody3D
{
	public Node3D TrailTarget;
	[Export]
	public Node3D Trail;

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
		if (Trail != null && TrailTarget != null)
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

	public void OnBodyEntered(Node3D body)
	{
		QueueFree();
	}

	private static Vector3 Perpendicular(Vector3 orig)
	{
		return orig.Z < orig.X ? new Vector3(orig.Y, -orig.X, 0) : new Vector3(0, -orig.Z, orig.Y);
	}
}
