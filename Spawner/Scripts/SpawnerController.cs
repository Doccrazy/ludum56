using Godot;
using Godot.Collections;

public partial class SpawnerController : Node3D
{
	[Export]
	public int MaxSpawningRate = 10;

	[Export]
	public PackedScene SpawnScene;


	private readonly Array<Node3D> _spawners = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		var children = GetChildren();


		foreach (Node node in children)
		{
			var groups = node.GetGroups();

			if (groups.Contains("Spawner") && IsInstanceValid(node))
			{
				_spawners.Add((Node3D)node);
			}

			GD.Print("Spawner count: ", _spawners.Count);
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("spawn_debug"))
		{
			Spawn();
		}
	}

	public void Spawn()
	{
		var spawnPerBlock = MaxSpawningRate / _spawners.Count;

		foreach (Spawner spawner in _spawners)
		{
			spawner.SpawnElements(SpawnScene, spawnPerBlock);
		}

	}

}
