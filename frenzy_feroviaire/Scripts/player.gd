extends CharacterBody2D

@export var inventory : Inventory
const SPEED = 300.0
const ACCELERATION = 100
const JUMP_VELOCITY = -400.0


func _physics_process(delta: float) -> void:
	var direction := Input.get_vector("left", "right", "up","down")
	velocity.x = move_toward(velocity.x, direction.x * SPEED, ACCELERATION)
	velocity.y = move_toward(velocity.y, direction.y * SPEED, ACCELERATION)

	move_and_slide()
