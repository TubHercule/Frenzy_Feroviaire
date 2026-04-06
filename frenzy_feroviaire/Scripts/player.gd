extends CharacterBody2D

@export var inventory : Inventory
@onready var tilemap : TileMap = $"../../TileMap"
const SPEED = 300.0
const ACCELERATION = 100
const JUMP_VELOCITY = -400.0


func _physics_process(delta: float) -> void:
	if Input.is_action_just_pressed("take"):
		try_take()
		
	var direction := Input.get_vector("left", "right", "up","down")
	velocity.x = move_toward(velocity.x, direction.x * SPEED, ACCELERATION)
	velocity.y = move_toward(velocity.y, direction.y * SPEED, ACCELERATION)

	move_and_slide()

func try_take():
	var cell = tilemap.local_to_map(global_position)
	inventory.take_items(cell)
