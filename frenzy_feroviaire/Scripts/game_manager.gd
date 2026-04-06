extends Node2D
@onready var tilemap = get_tree().get_root().find_child("TileMap", true, false) #$"../TileMap"
@onready var objects_container = get_tree().get_root().find_child("obj_container", true, false) #$"../obj_container"

var objects_map : Dictionary = {}
var cell_obj : Dictionary = {"type": 0, "nb": 0}
# key = Vector2i (cell)
# value = Array d’objets présents sur la tile

func add_object(cell: Vector2i, type : Types.CarryType, nb : int):
	if not objects_map.has(cell):
		objects_map[cell] = Cell_Obj.new(type, 0)
	
	objects_map[cell].nb += nb


func remove_object(cell: Vector2i, type : Types.CarryType, nb : int):
	if not objects_map.has(cell):
		return
	
	objects_map[cell].nb -= nb
	
	if objects_map[cell].nb <= 0:
		objects_map.erase(cell)
		

# MAP GENERATION
#-------------------

var tree_scenes = [
	preload("res://scenes/tree_1.tscn"),
	preload("res://scenes/tree_2.tscn"),
	preload("res://scenes/tree_3.tscn"),
	preload("res://scenes/tree_4.tscn"),
	preload("res://scenes/tree_5.tscn")
]

var stackable_items_scenes = {
	Types.CarryType.WOOD : preload("res://scenes/wood.tscn")
}

var tree_scene = preload("res://scenes/tree_2.tscn")

const SOURCE_ID = 0 # ID de ton atlas (à adapter)
const TILE_FOREST = Vector2i(1, 0)
const TILE_PLAIN = Vector2i(0, 0)

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
			spawn_item(cell, Types.CarryType.WOOD)

func spawn_item(cell: Vector2i, item_scene_id : Types.CarryType):
	var Stackable_Item = stackable_items_scenes[item_scene_id].instantiate()
	# hauteur du sprite
	var sprite = Stackable_Item.get_node("Sprite2D")
	var height = sprite.texture.get_height()

	# position centrée sur la tile
	Stackable_Item.position = tilemap.map_to_local(cell) - Vector2(0, height / 2)
	add_object(cell, Stackable_Item.type, 1)
	objects_container.add_child(Stackable_Item)
	
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
