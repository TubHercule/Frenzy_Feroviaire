using Godot;
using System;

public partial class Item : Node
{
	private Types.CarryType type;
	private int nb = 0;
	private int maxNb;
	//public Item(int nb){this.nb = nb;}

	public Item(Types.CarryType type, int maxStack, int nb = 0)
	{
		this.type = type;
		this.maxNb = maxStack;
		this.nb = nb;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public Item add(Item item)
	{
		if (item == null)
			return null;
		if (item.type != type)
			throw new ItemTypeException("Cannot add item of different type");	
		if (nb + item.nb > maxNb){
			int dif = nb + item.nb - maxNb;
			nb = maxNb;
			return new Item(type, maxNb, dif);
		}
		nb += item.nb;
		return null;
	}

	public Item sub(Item item)
	{
		if (item == null)
			return null;
		if (item.type != type)
			throw new ItemTypeException("Cannot add item of different type");
		nb -= item.nb;
		if (nb < 0)
		{
			int dif = nb;
			nb = 0;
			return new Item(type, maxNb, dif);
		}
		return null;
	}

	public int getNb()
	{
		return nb;
	}
	public void setNb(int nb)
	{
		this.nb = nb;
	}
	public int getMaxNb()
	{
		return maxNb;
	}
	
	public void setMaxNb(int max_nb)
	{
		this.maxNb = max_nb;
	}


	public Types.CarryType getType()
	{
		return type;
	}
	public void setType(Types.CarryType type)
	{
		this.type = type;
	}
	
}
