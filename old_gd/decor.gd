extends Node2D
class_name Decor

@export var max_health = 30;
@export var decor_name = "";
@export var hurtbox = Area2D;
@export var decor_sprite = Sprite2D;
@export var dropped_item_type : Types.CarryType;
var decor_type : Types.WorldObjectType
var health
var cell : Vector2i

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	y_sort_enabled = true
	health = max_health

func take_damage(damage : int) -> void :
	print("damage : ",damage)
	health -= damage
	if health <= 0:
		Game_Manager.add_object(self.cell, dropped_item_type, 1)
		queue_free()
		
