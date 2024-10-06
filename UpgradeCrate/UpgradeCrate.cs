using Godot;
using System;

public partial class UpgradeCrate : Node3D
{
	[Export]
	public PackedScene ContainedWeapon;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnBodyEntered(Node3D body)
	{
		if (body is IWeaponHolder)
		{
			if (ContainedWeapon != null)
			{
				var holder = body as IWeaponHolder;
				holder.SwitchWeapon(ContainedWeapon.Instantiate<Weapon>());
			}
			else
			{
				GD.Print("No contained weapon set for crate");
			}
			QueueFree();
		}
	}
}
