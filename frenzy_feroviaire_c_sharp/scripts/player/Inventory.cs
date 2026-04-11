using Godot;
using System;

public partial class Inventory : Node2D
{
	
	private Player player; 
	private Item item;
	private int capacity = 5;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = (Player)this.GetParent();
	}

	public Item addItems(Item _item)
	{
		if (item == null)
		{
			item = ItemManager.instance.createItem(_item.getType());
			item.setMaxNb(capacity);
		}
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
	public Item getItem()
	{
		return item;
	}
	public bool is_empty()
	{
		return item == null;
	}

}
