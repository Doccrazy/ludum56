using Godot;
using System;

public partial class PissDecal : Node3D
{
	private double Time = 0f;
	private const double LifeTime = 10f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Time += delta;
		GetNode<Decal>("Decal").Modulate = new Color(1, 1, 1, (float)(Math.Max(LifeTime - Time, 0) / LifeTime));
		if (Time >= LifeTime)
		{
			QueueFree();
		}
	}
}
