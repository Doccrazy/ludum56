using Godot;
using System;

public partial class PissEmitter : Node3D, IWeapon
{
	[Export]
	protected PackedScene PissParticleScene;
	[Export]
	protected Node3D EmitterNode;
	private PissParticle _lastEmitted;

	public const float PissVelocity = 3f;

	public bool Emitting { get; set; }

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
		if (!Emitting)
		{
			if (_lastEmitted != null)
			{
				ResetTrail();
			}
			return;
		}
		EmitParticle();
	}

	private void EmitParticle()
	{
		var direction = (EmitterNode.GlobalTransform.Basis * new Vector3(0, 0, 1)).Normalized();
		var pissParticle = PissParticleScene.Instantiate<PissParticle>();
		pissParticle.Transform = EmitterNode.GlobalTransform;
		pissParticle.LinearVelocity = direction * PissVelocity;
		pissParticle.TrailTarget = EmitterNode;
		if (_lastEmitted != null)
		{
			_lastEmitted.TrailTarget = pissParticle;
		}
		GetTree().CurrentScene.AddChild(pissParticle);
		_lastEmitted = pissParticle;
	}

	private void ResetTrail()
	{
		_lastEmitted.TrailTarget = null;
		_lastEmitted = null;
	}
}
