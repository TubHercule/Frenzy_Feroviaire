using Godot;
using System;
using System.Collections.Generic;

public partial class ItemManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Item createItem(Types.CarryType type)
	{
		switch(type)
		{	
			case Types.CarryType.WOOD:
				return new Resource(type,9);
			case Types.CarryType.STONE:
				return new Resource(type,9);
			case Types.CarryType.COAL:
				return new Resource(type,9);
			case Types.CarryType.AXE:
				List<Types.WorldObjectType> canDestroy = [Types.WorldObjectType.TREE];
				return new Tool(type, canDestroy, 10);
			case Types.CarryType.PICKAXE:
				List<Types.WorldObjectType> canDestroy2 = [Types.WorldObjectType.ROCK];
				return new Tool(type, canDestroy2, 10);
			default:
				return null;
		}
			
	}
}
