using Godot;
using System;

public partial class Healthbar : Sprite3D
{
	[Export]
	public Texture2D BarGreen;
	[Export]
	public Texture2D BarYellow;
	[Export]
	public Texture2D BarRed;
	private IDamageable _parent;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var parent = GetParentOrNull<Node>();
		while (parent != null && !(parent is IDamageable))
		{
			parent = parent.GetParentOrNull<Node>();
		}
		_parent = parent as IDamageable;
		Texture = GetNode<SubViewport>("SubViewport").GetTexture();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_parent != null)
		{
			UpdateBar(_parent.Life, _parent.MaxLife);
		}
	}

	private void UpdateBar(int amount, int full)
	{
		var bar = GetNode<TextureProgressBar>("SubViewport/TextureProgressBar");
		bar.TextureProgress = BarGreen;
		if (amount < 0.75 * full)
		{
			bar.TextureProgress = BarYellow;
		}
		if (amount < 0.45 * full)
		{
			bar.TextureProgress = BarRed;
		}
		bar.MaxValue = full;
		bar.Value = amount;
	}
}
