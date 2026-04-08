extends CharacterBody2D

@export var inventory : Inventory
@onready var tilemap : TileMap = $"../../TileMap"
const SPEED = 300.0
const ACCELERATION = 100
const JUMP_VELOCITY = -400.0
var target_range : int = 16
var direction := Vector2.ZERO

	
func _ready() -> void:
	add_to_group("players")

func _physics_process(delta: float) -> void:
	if Input.is_action_just_pressed("take"):
		if get_carry_type() == Types.CarryType.NONE:
			try_take()
		else:
			try_drop()
			
	direction = Input.get_vector("left", "right", "up","down")
	velocity.x = move_toward(velocity.x, direction.x * SPEED, ACCELERATION)
	velocity.y = move_toward(velocity.y, direction.y * SPEED, ACCELERATION)
	update_interaction_point()

	move_and_slide()

func try_take():
	var cell = tilemap.local_to_map(global_position)
	inventory.take_items(cell)

func try_drop():
	var cell = tilemap.local_to_map(global_position)
	inventory.drop_items(cell)
	
func get_carry_type() -> Types.CarryType:
	if inventory.cell_obj_container != null:
		return inventory.cell_obj_container.type
	else:
		return Types.CarryType.NONE

func update_interaction_point():
	var offset = Vector2.ZERO

	if direction.x > 0:
		offset = Vector2(target_range, 0)
	elif direction.x < 0:
		offset = Vector2(-target_range, 0)
	elif direction.y > 0:
		offset = Vector2(0, target_range)
	elif direction.y < 0:
		offset = Vector2(0, -target_range)
	$target.position = offset

func _on_target_body_entered(body: Node2D) -> void:
	var entity = body.get_parent()
	#if Types.is_tool(inventory.carry_type):
		
