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

	private PackedScene itemDisplayerScene = GD.Load<PackedScene>("res://scenes/ItemDisplayer.tscn");

	private const int SOURCE_ID = 0;
	private readonly Vector2I TILE_FOREST = new Vector2I(1, 0);
	private readonly Vector2I TILE_PLAIN = new Vector2I(0, 0);
	private readonly Vector2I TILE_ROCK = new Vector2I(0, 1);

	private PackedScene[] tree_scenes =
	{
		GD.Load<PackedScene>("res://scenes/tree_6.tscn")/*,
		GD.Load<PackedScene>("res://scenes/tree_2.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_3.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_4.tscn"),
		GD.Load<PackedScene>("res://scenes/tree_5.tscn")*/
	};

	public override void _Ready()
	{
		Instance = this; //Singleton
		_tilemap = GetTree().Root.FindChild("TileMap", true, false) as TileMap;
		_objectsContainer = GetTree().Root.FindChild("obj_container", true, false);

		if (_tilemap == null || _objectsContainer == null)
		{
			GD.PrintErr("_tilemap ou _objectsContainer est null. Vérifiez les noms de noeuds dans la scène.");
			return;
		}

		if (ItemManager.instance == null)
		{
			GD.PrintErr("ItemManager.instance est null : vérifiez que le noeud ItemManager existe et qu'il est présent dans la scène.");
			return;
		}

		spawnAll();
		GD.Print(itemsMap);
	}

	/*private Node instantiateContainer(Types.CarryType type, int max_nb = -1)
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
	}*/

	private Item instantiateCell(Vector2I cell, Item _item)
	{
		if (_item == null)
			return null;
		
		ItemDisplayer itemDisplayer = (ItemDisplayer)itemDisplayerScene.Instantiate();
		Item item = ItemManager.instance.createItem(_item.getType());
		Item rest = item.add(_item);
		itemDisplayer.setItem(item);
		itemsMap[cell] = itemDisplayer;

		itemDisplayer.Set("position", _tilemap.MapToLocal(cell));
		_objectsContainer.AddChild(itemDisplayer);
		return rest;
	}

	public Item getItemInCell(Vector2I cell)
	{
		if (!itemsMap.ContainsKey(cell))
			return null;

		ItemDisplayer itemDisplayer = itemsMap[cell];
		return itemDisplayer.getItem();
	}

	public Item addItemsInCell(Vector2I cell, Item _item)
	{
		Item rest;
		if (!itemsMap.ContainsKey(cell))
		{
			if (_item == null)
				return null;
			
			rest = instantiateCell(cell, _item);
			if (rest == null)
			{
				GD.Print("item rest : 0");
			}else{
				GD.Print("item rest : ",rest.getNb());
			}
			
			return rest;
		}
		ItemDisplayer itemDisplayer = itemsMap[cell];
		rest = itemDisplayer.addItems(_item);
		if (rest == null)
		{
			GD.Print("item rest : 0");
		}
		else
		{
			GD.Print("item rest : ",rest.getNb());
		}
		return rest;
	}

	public Item subItemInCell(Vector2I cell, Item item)
	{
		if (!itemsMap.ContainsKey(cell))
			return item;

		ItemDisplayer itemDisplayer = itemsMap[cell];
		Item rest = itemDisplayer.subItems(item);
		if (rest == null){
			removeCellFromMap(cell);
		}
		return rest;
	}

	public void removeCellFromMap(Vector2I cell){
		if (itemsMap.ContainsKey(cell))
		{
			itemsMap[cell].QueueFree();
			itemsMap.Remove(cell);
		}
	}
	public void setItemInCell(Vector2I cell, Item _item)
	{
		if (!itemsMap.ContainsKey(cell))
		{
			if (_item == null)
				return;

			instantiateCell(cell, _item);
		}

		if (_item == null){
			removeCellFromMap(cell);
		}
		else
			itemsMap[cell].setItem(_item);
	}
	private void spawnAll()
	{
		var usedCells = _tilemap.GetUsedCells(0);

		foreach (Vector2I cell in usedCells)
		{
			Vector2I atlasCoords = _tilemap.GetCellAtlasCoords(0, cell);

			if (atlasCoords == TILE_FOREST)
				createDecor(cell, Types.WorldObjectType.TREE);

			else if (atlasCoords == TILE_PLAIN){
				Item item = ItemManager.instance.createItem(Types.CarryType.WOOD,2);
				setItemInCell(cell, item);
			}

			/*else if (atlasCoords == TILE_ROCK){
				Item item = ItemManager.instance.createItem(Types.CarryType.AXE,1);
				setItemInCell(cell, item);
			}*/
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
