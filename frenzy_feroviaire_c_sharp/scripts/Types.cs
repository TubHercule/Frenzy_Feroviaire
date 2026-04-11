using Godot;
using System;
using System.Collections.Generic;

public static class Types
{
	// Enum équivalent à CarryType
	public enum CarryType
	{
		NONE,
		WOOD,
		STONE,
		COAL,
		AXE,
		PICKAXE
	}

	// Vérifie si c'est un outil
    public static bool IsTool(CarryType type)
    {
        return type == CarryType.AXE || type == CarryType.PICKAXE;
    }

	// Vérifie si c'est une ressource
	public static bool IsResource(CarryType type)
	{
		return type == CarryType.WOOD || type == CarryType.STONE || type == CarryType.COAL;
	}

	// Enum équivalent à WorldObjectType
	public enum WorldObjectType
	{
		NONE,
		TREE,
		ROCK,
		COAL
	}

	public static Dictionary<Types.CarryType, PackedScene> itemsScenes = new Dictionary<Types.CarryType, PackedScene>
	{
		{ Types.CarryType.WOOD, GD.Load<PackedScene>("res://scenes/wood.tscn") },
		{ Types.CarryType.AXE, GD.Load<PackedScene>("res://scenes/axe.tscn") }
	};
}
