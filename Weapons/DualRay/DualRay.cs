using Godot;
using System;

public partial class DualRay : Weapon
{
	[Export]
	public Node[] Emitters;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		foreach (var emitter in Emitters)
		{
			(emitter as Weapon).Emitting = Emitting;
		}
	}
}
