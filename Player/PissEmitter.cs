using Godot;
using System;

public partial class PissEmitter : Node3D, IWeapon
{
	[Export]
	protected PackedScene PissParticleScene;
	private PissParticle LastEmitted;

	private const float PissVelocity = 3f;

	private bool _Emitting = false;
	public bool Emitting { get => _Emitting; set => _Emitting = value; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
	}

	public void OnTimerTimeout()
	{
		if (!_Emitting)
		{
			if (LastEmitted != null)
			{
				LastEmitted.TrailTarget = null;
				LastEmitted = null;
			}
			return;
		}
		var direction = (GlobalTransform.Basis * new Vector3(0, 0, 1)).Normalized();
		var pissParticle = PissParticleScene.Instantiate<PissParticle>();
		pissParticle.Transform = GlobalTransform;
		pissParticle.LinearVelocity = direction * PissVelocity;
		pissParticle.TrailTarget = this;
		if (LastEmitted != null)
		{
			LastEmitted.TrailTarget = pissParticle;
		}
		GetTree().CurrentScene.AddChild(pissParticle);
		LastEmitted = pissParticle;
	}
}
