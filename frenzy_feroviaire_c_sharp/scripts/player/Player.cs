using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export] public Inventory inventory;

	private TileMap tilemap;

	private const float SPEED = 300.0f;
	private const float ACCELERATION = 100.0f;

	private int target_range = 16;

	private Vector2 direction = Vector2.Zero;

	private Node2D current_target;
	private dynamic tool;

	private Timer timer;
	private Area2D target;

	public override void _Ready()
	{
		AddToGroup("players");

		tilemap = GetNode<TileMap>("../../TileMap");
		timer = GetNode<Timer>("Timer");
		target = GetNode<Area2D>("target");

		// 🔔 Connexion du signal Timer
		timer.Timeout += OnTimerTimeout;

		// 🔔 Connexion des signaux de collision (Area2D)
		target.BodyEntered += OnTargetBodyEntered;
		target.BodyExited += OnTargetBodyExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("take"))
		{
			if (inventory.is_empty())
				tryTake();
			else
				tryDrop();
		}

		direction = Input.GetVector("left", "right", "up", "down");

		Velocity = new Vector2(
			Mathf.MoveToward(Velocity.X, direction.X * SPEED, ACCELERATION),
			Mathf.MoveToward(Velocity.Y, direction.Y * SPEED, ACCELERATION)
		);

		if (direction != Vector2.Zero)
			UpdateInteractionPoint();

		MoveAndSlide();
	}

	public void tryTake()
	{
		Vector2I cell = tilemap.LocalToMap(GlobalPosition);
		Item item = GameManager.Instance.getItemInCell(cell);
		if (item == null)
			return;

		Item itemRest = inventory.addItems(item);
		GameManager.Instance.setItemInCell(cell, itemRest);
		
	}

	public void tryDrop()
	{
		Vector2I cell = tilemap.LocalToMap(GlobalPosition);
		Item item = inventory.getItem();
		if (item == null)
			return;

		Item itemRest = GameManager.Instance.subItemInCell(cell, item);

	}

	public Types.CarryType getCarryType()
	{
		if (inventory.getItem() != null)
			return inventory.getItem().getType();

		return Types.CarryType.NONE;
	}
	

	private void UpdateInteractionPoint()
	{
		Vector2 offset = Vector2.Zero;

		if (direction.X > 0)
			offset = new Vector2(target_range, 0);
		else if (direction.X < 0)
			offset = new Vector2(-target_range, 0);
		else if (direction.Y > 0)
			offset = new Vector2(0, target_range);
		else if (direction.Y < 0)
			offset = new Vector2(0, -target_range);

		target.Position = offset;
	}

	private void OnTargetBodyEntered(Node2D body)
	{
		GD.Print(body.GetParent());

		if (tool != null)
		{
			GD.Print("I have a tool");

			if (body.GetParent().HasMethod("take_damage"))
			{
				GD.Print("I have a target");

				current_target = body;
				timer.Start();
			}
		}
	}

	private void OnTargetBodyExited(Node2D body)
	{
		if (body == current_target)
		{
			current_target = null;
			timer.Stop();
		}
	}

	private void OnTimerTimeout()
	{
		GD.Print("timer");

		if (current_target != null)
		{
			GD.Print("timer2 : ", tool.can_destroy);

			if (tool.can_destroy.Contains(current_target.GetParent().Get("decor_type")))
			{
				GD.Print("timer3");

				if (current_target.GetParent().HasMethod("take_damage"))
				{
					current_target.GetParent().Call("take_damage", tool.damage);
				}
				else
				{
					GD.Print("ATTENTION : l'objet n'a pas de fonction take damage");
				}
			}
		}
	}
}
