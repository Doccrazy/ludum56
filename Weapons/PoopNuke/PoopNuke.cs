using Godot;
using System;

public partial class PoopNuke : Weapon
{
	[Export]
	public PackedScene Projectile;

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
		if (Emitting)
		{
			var nuke = Projectile.Instantiate<NukeProjectile>();
			GetTree().CurrentScene.AddChild(nuke);
			nuke.GlobalPosition = GlobalPosition;
			Holder?.ResetWeapon();
			QueueFree();
		}
	}
}
