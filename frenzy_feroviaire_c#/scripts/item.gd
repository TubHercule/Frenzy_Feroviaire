extends Node2D
class_name Item
@export var type : Types.CarryType = Types.CarryType.NONE
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	y_sort_enabled = true


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
