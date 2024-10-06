using Godot;
using System;

public partial class NukeProjectile : Node3D
{
	[Export]
	public int Damage = 100;
	private bool _done;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!_done)
		{
			BringDoom();
			_done = true;
		}
	}

	private async void BringDoom()
	{
		GetNode<GpuParticles3D>("GPUParticles3D").Emitting = true;
		var bodies = GetNode<Area3D>("Area3D").GetOverlappingBodies();
		foreach (var body in bodies)
		{
			if (body is IDamageable)
			{
				(body as IDamageable).TakeDamage(Damage);
			}
		}
		await ToSignal(GetTree().CreateTimer(2), "timeout");
		QueueFree();
	}
}
