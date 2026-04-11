using Godot;
using System;
using System.Collections.Generic;

public partial class ItemManager : Node2D
{
	public static ItemManager instance;

	public override void _EnterTree()
	{
		instance = this; // Singleton early initialization
	}

	public override void _Ready()
	{
		if (instance == null)
			instance = this; //Singleton
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Item createItem(Types.CarryType type, int nb = 0)
	{
		switch(type)
		{	
			case Types.CarryType.WOOD:
				return new Resource(type,9,nb);
			case Types.CarryType.STONE:
				return new Resource(type,9,nb);
			case Types.CarryType.COAL:
				return new Resource(type,9,nb);
			case Types.CarryType.AXE:
				List<Types.WorldObjectType> canDestroy = [Types.WorldObjectType.TREE];
				return new Tool(type, canDestroy, 10, nb);
			case Types.CarryType.PICKAXE:
				List<Types.WorldObjectType> canDestroy2 = [Types.WorldObjectType.ROCK];
				return new Tool(type, canDestroy2, 10, nb);
			default:
				return null;
		}
			
	}
}
