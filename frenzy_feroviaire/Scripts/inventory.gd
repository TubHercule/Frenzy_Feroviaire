extends Node2D
class_name Inventory

var carry_type : Types.CarryType = Types.CarryType.NONE
var amount : int = 0
var max_amount : int = 5

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.

func take_items(cell : Vector2i):
	var cell_item : Cell_Obj
	print(cell)
	if Game_Manager.objects_map.has(cell):
		cell_item = Game_Manager.objects_map[cell]
		
	if carry_type == Types.CarryType.NONE:
		carry_type = cell_item.type
		
	if  carry_type == cell_item.type:
		if Types.is_tool(cell_item.type) and amount == 0:
			amount += 1
			Game_Manager.remove_object(cell, cell_item.type, 1)
		elif Types.is_resource(cell_item.type):
			var amount_taken = min(max_amount - amount, cell_item.nb)
			amount += amount_taken
			Game_Manager.remove_object(cell, cell_item.type, amount_taken)
	else:
		print("Incompatible item")
		return
	
		
		
	"""for item in items:
		if carry_type != Types.CarryType.NONE:
			carry_type  = item.type
			items.append(item)
			Game_Manager.remove_object(cell, item)
			is_tool = Types.is_tool(item.type)
		elif items.size() < max_amount and item.type == carry_type:
			items.append(item)
			Game_Manager.remove_object(cell, item)
		else:
			print("items full!")"""
	print(carry_type)
	print(amount)

func drop_items(item : Item):
	if carry_type != Types.CarryType.NONE:
		carry_type = Types.CarryType.NONE
		amount = 0

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
