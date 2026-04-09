using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node2D
{
	public static GameManager Instance;
	
	private TileMap _tilemap;
	private Node _objectsContainer;

	private Dictionary<Vector2I, Node> objects_map = new();

	private PackedScene cell_stackable_items_container_scene = GD.Load<PackedScene>("res://scenes/cell_stackable_items_container.tscn");
	private PackedScene cell_tools_container_scene = GD.Load<PackedScene>("res://scenes/cell_tools_container.tscn");

	private const int SOURCE_ID = 0;
	private readonly Vector2I TILE_FOREST = new Vector2I(1, 0);
	private readonly Vector2I TILE_PLAIN = new Vector2I(0, 0);
	private readonly Vector2I TILE_ROCK = new Vector2I(0, 1);

	private PackedScene[] tree_scenes =
	{
		GD.Load<PackedScene>("res://scenes/tree_1.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_2.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_3.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_4.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_5.tscn")
	};

	public override void _Ready()
	{
		Instance = this; //Singleton
		_tilemap = GetTree().Root.FindChild("TileMap", true, false) as TileMap;
		_objectsContainer = GetTree().Root.FindChild("obj_container", true, false);

		spawnAll();
		GD.Print(objects_map);
	}

	private Node instantiateContainer(Types.CarryType type, int max_nb = -1)
	{
		Node cell_obj_container;

		if (Types.IsTool(type))
		{
			cell_obj_container = cell_tools_container_scene.Instantiate();

			Node item = stackableItemsScenes[type].Instantiate();
			cell_obj_container.Call("init", type, item.Get("can_destroy"), item.Get("damage"));
			item.QueueFree();
		}
		else
		{
			cell_obj_container = cell_stackable_items_container_scene.Instantiate();
			cell_obj_container.Call("init", type);

			if (max_nb != -1)
				cell_obj_container.Call("set_max_nb", max_nb);
		}

		return cell_obj_container;
	}

	private void instantiateCell(Vector2I cell, Types.CarryType type)
	{
		Node cell_obj_container = instantiateContainer(type);

		objects_map[cell] = cell_obj_container;

		cell_obj_container.Set("position", _tilemap.MapToLocal(cell));
		_objectsContainer.AddChild(cell_obj_container);
	}

	public int addObject(Vector2I cell, Types.CarryType type, int nb)
	{
		if (!objects_map.ContainsKey(cell))
		{
			instantiateCell(cell, type);
		}
		else
		{
			if ((Types.CarryType)objects_map[cell].Get("type") != type)
			{
				GD.Print("ATTENTION : types différents sur la même tuile");
				return nb;
			}
		}

		int dif = (int)objects_map[cell].Call("add_items", nb);
		GD.Print("dif : ", dif);
		return dif;
	}

	public void removeObject(Vector2I cell, Types.CarryType type, int nb)
	{
		if (!objects_map.ContainsKey(cell))
			return;

		if ((Types.CarryType)objects_map[cell].getType() != type)
		{
			GD.Print("ATTENTION : types différents sur la même tuile");
		}

		GD.Print("rm obj");
		objects_map[cell].Call("add_items", -nb);

		if ((int)objects_map[cell].Get("nb") <= 0)
		{
			objects_map[cell].QueueFree();
			objects_map.Remove(cell);
		}
	}

	private void spawnAll()
	{
		var usedCells = _tilemap.GetUsedCells(0);

		foreach (Vector2I cell in usedCells)
		{
			Vector2I atlasCoords = _tilemap.GetCellAtlasCoords(0, cell);

			if (atlasCoords == TILE_FOREST)
				createDecor(cell, Types.WorldObjectType.TREE);

			if (atlasCoords == TILE_PLAIN)
				addObject(cell, Types.CarryType.WOOD, 2);

			if (atlasCoords == TILE_ROCK)
				addObject(cell, Types.CarryType.AXE, 1);
		}
	}

	private void createDecor(Vector2I cell, Types.WorldObjectType decor_type)
	{
		switch (decor_type)
		{
			case Types.WorldObjectType.TREE:
				PackedScene tree_scene = tree_scenes[GD.RandRange(0, tree_scenes.Length - 1)];
				Decor tree = (Decor)tree_scene.Instantiate<Node2D>();
				Sprite2D sprite = tree.GetNode<Sprite2D>("Sprite2D");
				float height = sprite.Texture.GetHeight();
				tree.Position = _tilemap.MapToLocal(cell) - new Vector2(0, height / 3);
				tree.init(Types.WorldObjectType.TREE, Types.CarryType.WOOD, cell);
				_objectsContainer.AddChild(tree);
				break;
			case Types.WorldObjectType.ROCK:
				//TODO : spawn rock
				break;
			default : //ne rien faire
				break;
		}
	}

}
