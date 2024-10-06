using Godot;

public partial class Spawner : Node3D
{
	[Export]
	public bool enabled = true;

	private CollisionShape3D _collisionShape3D;
	private BoxShape3D _boxShape3D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_collisionShape3D = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");
		_boxShape3D = (BoxShape3D)_collisionShape3D.Shape;
	}


	public void SpawnElements(PackedScene scene, int amount = 1)
	{
		if (!enabled)
		{
			return;
		}
		for (int i = 0; i < amount; i++)
		{


			Vector3 size = _boxShape3D.Size;

			float randomX = (float)GD.RandRange(-size.X / 2, size.X / 2);
			float randomZ = (float)GD.RandRange(-size.Z / 2, size.Z / 2);
			int Y = 0;


			Vector3 randomPosition = new Vector3(randomX, Y, randomZ);

			Node3D instance = (Node3D)scene.Instantiate();

			instance.GlobalTransform = new Transform3D(Basis.Identity, randomPosition + GlobalTransform.Origin);

			GetTree().Root.AddChild(instance);

		}
	}
}
