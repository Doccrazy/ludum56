using Godot;

public abstract partial class Weapon : Node3D
{
  public virtual bool Emitting { get; set; }

  public IWeaponHolder Holder { get; set; }
}