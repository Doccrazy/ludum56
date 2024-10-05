using Godot;
using System;

public partial class Enemy : Node
{
	[Export]
	protected bool isActive = false;

	[Export]
	protected int speed = 10;

	[Export]
	protected bool isDead = false;

	[Export]
	protected float damage = 10f;

	[Export]
	protected CharacterBody3D character;


	// Called when the node enters the scene tree for the first time.p
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
