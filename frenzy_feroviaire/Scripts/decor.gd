extends Node2D
class_name Decor

@export var max_health = 30;
@export var decor_name = "";
@export var hurtbox = Area2D;
@export var decor_sprite = Sprite2D;

var health
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	y_sort_enabled = true
	health = max_health

func take_damage(damage : int) -> void :
	health -= damage
	if health <= 0:
		queue_free()
