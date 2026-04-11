using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

public partial class Tool : Item
{
	public List<Types.WorldObjectType> canDestroy = new List<Types.WorldObjectType>();
	int damage;
	public Tool(Types.CarryType type, List<Types.WorldObjectType> canDestroy, int damage, int maxStack = 1, int nb = 0) : base(type, maxStack, nb)
	{
		this.canDestroy = canDestroy;
		this.damage = damage;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public List<Types.WorldObjectType> getCanDestroy()
	{
		return canDestroy;
	}

	public int getDamage()
	{
		return damage;
	}

	public void use(Decor decor)
	{
		if (canDestroy.Contains(decor.getType()))
		{
			decor.takeDamage(damage);
		}
	}


}
