extends Item
class_name Harvesting_Tool

@export var can_destroy : Array[Types.WorldObjectType] = []

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	super()
