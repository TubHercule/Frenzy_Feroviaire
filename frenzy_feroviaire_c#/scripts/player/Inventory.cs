using Godot;
using System;

public partial class Inventory : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	private Player player = (Player)GetParent();
	private Item item;
	private int capacity = 5;

	public Item addItems(Item _item)
	{
		return item.add(_item);
	}

	public Item subItems(Item _item)
	{
		Item rest = item.sub(_item);
		if (item.getNb() <= 0)
		{
			item.QueueFree();
			item = null;
		}
		return rest;
	}
	public Item GetItem()
	{
		return item;
	}

}
