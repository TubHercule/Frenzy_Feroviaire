using Godot;
using System;

public partial class Resource : Item
{
	public Resource(Types.CarryType type, int maxStack, int nb = 0) : base(type, maxStack, nb)
	{
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
