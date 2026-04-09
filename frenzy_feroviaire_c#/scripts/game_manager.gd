extends Node2D
@onready var tilemap = get_tree().get_root().find_child("TileMap", true, false) #$"../TileMap"
@onready var objects_container = get_tree().get_root().find_child("obj_container", true, false) #$"../obj_container"

var objects_map : Dictionary = {}
var cell_obj : Dictionary = {"type": 0, "nb": 0}
# key = Vector2i (cell)
# value = Array d’objets présents sur la tile
@onready var cell_stackable_items_container_scene = preload("res://scenes/cell_stackable_items_container.tscn")
@onready var cell_tools_container_scene = preload("res://scenes/cell_tools_container.tscn")

const SOURCE_ID = 0 # ID de ton atlas (à adapter)
const TILE_FOREST = Vector2i(1, 0)
const TILE_PLAIN = Vector2i(0, 0)
const TILE_ROCK = Vector2i(0, 1)

var tree_scenes = [
	preload("res://scenes/tree_1.tscn"),
	preload("res://scenes/tree_2.tscn"),
	preload("res://scenes/tree_3.tscn"),
	preload("res://scenes/tree_4.tscn"),
	preload("res://scenes/tree_5.tscn")
]

var stackable_items_scenes = {
	Types.CarryType.WOOD : preload("res://scenes/wood.tscn"),
	Types.CarryType.AXE : preload("res://scenes/axe.tscn")
}

# create a cell if cell is not refferenced in objects_map
"""func get_cell(cell):
	if Game_Manager.objects_map[cell].has(cell):
		return Game_Manager.objects_map[cell]
	else:
		
		return Game_Manager.objects_map[cell]"""


func instantiate_container(type : Types.CarryType, max_nb : int = -1):
	var cell_obj_container
	if Types.is_tool(type):
		cell_obj_container = cell_tools_container_scene.instantiate()
		var item = stackable_items_scenes[type].instantiate()
		cell_obj_container.init(type, item.can_destroy, item.damage)
		item.queue_free()
	else:
		cell_obj_container = cell_stackable_items_container_scene.instantiate()
		cell_obj_container.init(type)
		if max_nb != -1:
			cell_obj_container.set_max_nb(max_nb)
	return cell_obj_container


func instantiate_cell(cell: Vector2i, type : Types.CarryType) -> void:
	var cell_obj_container = instantiate_container(type) #: Cell_Obj_Container
	objects_map[cell] = cell_obj_container
	cell_obj_container.position = tilemap.map_to_local(cell)
	objects_container.add_child(cell_obj_container)


func add_object(cell: Vector2i, type : Types.CarryType, nb : int) -> int:
	if not objects_map.has(cell):
		instantiate_cell(cell, type)
	else:
		if type != objects_map[cell].type:
			print("ATTENTION : types différents sur la même tuile")
			return nb 
	var dif = objects_map[cell].add_items(nb)
	print("dif : ",dif)
	return dif


func remove_object(cell: Vector2i, type : Types.CarryType, nb : int):
	if not objects_map.has(cell):
		return
	if type != objects_map[cell].type:
		print("ATTENTION : types différents sur la même tuile")
	print("rm obj")
	objects_map[cell].add_items(-nb)
	
	if objects_map[cell].nb <= 0:
		objects_map[cell].queue_free()
		objects_map.erase(cell)
		

# MAP GENERATION
#-------------------



func _ready():
	spawn_all()
	print(objects_map)

func spawn_all():
	var used_cells = tilemap.get_used_cells(0)

	for cell in used_cells:
		var atlas_coords = tilemap.get_cell_atlas_coords(0, cell)

		if atlas_coords == TILE_FOREST:
			spawn_tree(cell)
		if atlas_coords == TILE_PLAIN:
			add_object(cell, Types.CarryType.WOOD, 2)
		if atlas_coords == TILE_ROCK:
			add_object(cell, Types.CarryType.AXE, 1)

"""func spawn_item(cell: Vector2i, item_scene_id : Types.CarryType, nb : int):
	var Stackable_Item = stackable_items_scenes[item_scene_id].instantiate()
	# hauteur du sprite
	var sprite = Stackable_Item.get_node("Sprite2D")
	var height = sprite.texture.get_height()

	# position centrée sur la tile
	Stackable_Item.position = tilemap.map_to_local(cell) - Vector2(0, height / 2)
	add_object(cell, Stackable_Item)
	objects_container.add_child(Stackable_Item)"""
	
func spawn_tree(cell: Vector2i):
	var tree_scene = tree_scenes.pick_random()
	var tree = tree_scene.instantiate()
	# hauteur du sprite
	var sprite = tree.get_node("Sprite2D")
	var height = sprite.texture.get_height()
	
	tree.cell = cell
	# position centrée sur la tile
	tree.position = tilemap.map_to_local(cell) - Vector2(0, height / 3)

	objects_container.add_child(tree)
