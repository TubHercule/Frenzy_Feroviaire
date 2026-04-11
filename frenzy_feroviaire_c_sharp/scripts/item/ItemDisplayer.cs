using Godot;
using System;
using System.Collections.Generic;

public partial class ItemDisplayer : Node2D
{
	private Item item;
	private int stack_gap = 2;
	private List<Node2D> displayItemList = [];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		YSortEnabled = true;
	}

//return the number of items that can't be stacked
	public Item addItems(Item _item)
	{
		try
		{
			Item rest = this.item.add(_item);
			display();
			return rest;
		}
		catch (ItemTypeException e)
		{
			Console.WriteLine(e.Message);
			return item;
		}
	}

	public Item subItems(Item _item)
	{
		try
		{
			Item rest = item.sub(_item);
			if (item.isNeutral()){
				return rest;
			}
			else
			{
				display();
				return rest;
			}
			
		}
		catch (ItemTypeException e)
		{
			Console.WriteLine(e.Message);
			return _item;
		}
	}
	public void setItem(Item item)
	{
		this.item = item;
		display();
	}

	public void display()
	{
		if (item == null)
		{
			GD.PrintErr("ItemDisplayer.display() : item est null.");
			return;
		}
		PackedScene item_scene = Types.itemsScenes[item.getType()];
		int dif = item.getNb() - displayItemList.Count;
		int original_size = displayItemList.Count;
		Console.WriteLine("dif : {0}", dif);
		if (dif > 0)
		{
			for (int i = 0; i < dif; i++)
			{
				var displayItem = item_scene.Instantiate() as Node2D;
				displayItem.Position += new Vector2(0, -(i + original_size) * stack_gap);
				displayItemList.Add(displayItem);
				AddChild(displayItem);
			}
		}
		else
		{
			for (int i = 0; i < -dif; i++)
			{
				Console.WriteLine("obj removed");
				displayItemList[displayItemList.Count - 1].QueueFree();
				displayItemList.RemoveAt(displayItemList.Count - 1);
			}
		}
	}

	public void detect_entity(Area2D area){
		Console.Write("detected");
		Node entity = area.GetParent();
		if (entity.IsInGroup("players")){
			Player player = (Player)entity;
			if (player.getCarryType() == item.getType())
			{
				player.tryTake();
			}
		}
	}

	public Item getItem()
	{
		return item;
	}
}
