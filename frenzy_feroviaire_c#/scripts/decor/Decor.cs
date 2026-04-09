using Godot;
using System;

public partial class Decor : Node2D
{
	private int max_health = 30;
	private int health;
	private Types.WorldObjectType decor_type;
	private Types.CarryType dropped_item_type;
	private Area2D hurtbox;
	private Vector2I cell;

	public void init(Types.WorldObjectType decor_type, Types.CarryType dropped_item_type, Vector2I cell)
	{
		this.decor_type = decor_type;
		this.dropped_item_type = dropped_item_type;
		this.cell = cell;
	}
	public void takeDamage(int damage)
	{
		Console.WriteLine("damage : {0}", damage);
		health -= damage;
		if (health <= 0)
		{
			GameManager.Instance.addObject(cell, dropped_item_type, 1);
			QueueFree();
		}
	}
	public override void _Ready()
	{
		YSortEnabled = true;
		health = max_health;
	}

	public Types.WorldObjectType getType()
	{
		return decor_type;
	}

}
