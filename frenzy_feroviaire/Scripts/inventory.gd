extends Node2D
class_name Inventory

var carry_type : Types.CarryType = Types.CarryType.NONE
var amount : int = 0
var max_amount : int = 5
var is_tool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

func take_item(item : Item):
	if carry_type != Types.CarryType.NONE:
		carry_type  = item.type
		amount += 1
		is_tool = Types.is_tool(item.type)

func drop_items(item : Item):
	if carry_type != Types.CarryType.NONE:
		carry_type = Types.CarryType.NONE
		amount = 0
		pass
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
