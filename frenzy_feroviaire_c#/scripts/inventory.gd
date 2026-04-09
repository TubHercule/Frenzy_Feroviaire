extends Node2D
class_name Inventory

#var carry_type : Types.CarryType = Types.CarryType.NONE
#var amount : int = 0
#var max_amount : int = 5
@onready var player : CharacterBody2D = $".."
var cell_obj_container : Cell_Obj_Container 
var capacity : int = 5

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass

func player_full() -> bool:
	if cell_obj_container != null:
		return cell_obj_container.is_full()
	else:
		return false

func take_items(cell : Vector2i):
	if not player_full():
		var cell_item : Cell_Obj_Container
		print(cell)
		if Game_Manager.objects_map.has(cell):
			cell_item = Game_Manager.objects_map[cell]
		else:
			return
		
		if cell_obj_container == null: #inventaire vide
			cell_obj_container = Game_Manager.instantiate_container(cell_item.type, capacity)
			if Types.is_tool(cell_obj_container.type):
				player.tool = cell_obj_container
			
		if cell_obj_container.type == cell_item.type:
			#if Types.is_resource(cell_obj_container.type):
			var dif = cell_obj_container.add_items(cell_item.nb)
			print("nb_max : ",cell_obj_container.max_nb)
			Game_Manager.remove_object(cell, cell_item.type, cell_item.nb-dif)
			#elif Types.is_tool(cell_obj_container.type):
		else:
			print("Incompatible item")
			return

		print(cell_obj_container.type)
		print(cell_obj_container.nb)

func drop_items(cell : Vector2i):
	var dif = Game_Manager.add_object(cell, cell_obj_container.type, cell_obj_container.nb)
	print("dif : ",dif)
	if dif == 0:
		if Types.is_tool(cell_obj_container.type):
			player.tool = null
		cell_obj_container.queue_free()
		cell_obj_container = null
		
	if dif > 0: #la case a un exes
		cell_obj_container.nb = dif
		print("inv : ",cell_obj_container.nb)
	

"""
func get_tool():
	if cell_obj_container != null:
		if Types.is_tool(cell_obj_container.type):
			return cell_obj_container
	return null
"""


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
