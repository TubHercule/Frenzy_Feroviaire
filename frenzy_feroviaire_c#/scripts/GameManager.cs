using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class GameManager : Node2D
{
	public static GameManager Instance;
	
	private TileMap _tilemap;
	private Node _objectsContainer;

	private Dictionary<Vector2I, ItemDisplayer> itemsMap = new();
	private ItemManager itemManager;
	private PackedScene itemDisplayerScene = GD.Load<PackedScene>("res://scenes/ItemDisplayer.tscn");

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
		GD.Print(itemsMap);
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

	private Item instantiateCell(Vector2I cell, Item _item)
	{
		if (_item == null)
			return null;
		
		ItemDisplayer itemDisplayer = itemDisplayerScene.Instantiate();
		Item item = itemManager.createItem(_item.getType());
		Item rest = item.add(_item);
		itemDisplayer.setItem(item);
		itemsMap[cell] = 

		itemDisplayer.Set("position", _tilemap.MapToLocal(cell));
		_objectsContainer.AddChild(cell_obj_container);
	}

	public Item getItemOfCell(Vector2I cell)
	{
		if (!itemsMap.ContainsKey(cell))
			return null;

		ItemDisplayer itemDisplayer = itemsMap[cell];
		return itemDisplayer.getItem();
	}

	public Item addItemsInCell(Vector2I cell, Item _item)
	{
		if (!itemsMap.ContainsKey(cell))
		{
			if (_item == null)
				return null;

			instantiateCell(cell, _item);
		}
		Item item = getItemOfCell(cell);
		Item rest = item.add(_item);
		return rest;
	}

	public void subItemInCell(Vector2I cell, Item item)
	{
		if (!itemsMap.ContainsKey(cell))
			return;

		ItemDisplayer itemDisplayer = itemsMap[cell];
		int dif = itemDisplayer.subItems(item);

		if ((Types.CarryType)itemsMap[cell].getType() != type)
		{
			GD.Print("ATTENTION : types différents sur la même tuile");
		}

		GD.Print("rm obj");
		itemsMap[cell].Call("add_items", -nb);

		if ((int)itemsMap[cell].Get("nb") <= 0)
		{
			itemsMap[cell].QueueFree();
			itemsMap.Remove(cell);
		}
	}
	public void setItemInCell(Vector2I cell, Item item)
	{
		if (!itemsMap.ContainsKey(cell))
		{
			if (item == null)
				return;

			instantiateCell(cell);
		}

		itemsMap[cell].Call("set_item", item);

	private void spawnAll()
	{
		var usedCells = _tilemap.GetUsedCells(0);

		foreach (Vector2I cell in usedCells)
		{
			Vector2I atlasCoords = _tilemap.GetCellAtlasCoords(0, cell);

			if (atlasCoords == TILE_FOREST)
				createDecor(cell, Types.WorldObjectType.TREE);

			else if (atlasCoords == TILE_PLAIN)
				addObject(cell, Types.CarryType.WOOD, 2);

			else if (atlasCoords == TILE_ROCK)
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
