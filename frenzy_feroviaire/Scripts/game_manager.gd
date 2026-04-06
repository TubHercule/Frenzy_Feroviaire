extends Node2D
@onready var tilemap = $"../TileMap"
@onready var objects_container = $"../obj_container"

var objects_map : Dictionary = {}
# key = Vector2i (cell)
# value = Array d’objets présents sur la tile

func add_object(cell: Vector2i, obj : Item):
	if not objects_map.has(cell):
		objects_map[cell] = []
	
	objects_map[cell].append(obj)


func remove_object(cell: Vector2i, obj : Item):
	if not objects_map.has(cell):
		return
	
	objects_map[cell].erase(obj)
	
	if objects_map[cell].is_empty():
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

var tree_scene = preload("res://scenes/tree_2.tscn")

const SOURCE_ID = 0 # ID de ton atlas (à adapter)
const TILE_FOREST = Vector2i(1, 0) # coord de la tile forêt (à adapter)

func _ready():
	spawn_trees()

func spawn_trees():
	var used_cells = tilemap.get_used_cells(0)

	for cell in used_cells:
		var atlas_coords = tilemap.get_cell_atlas_coords(0, cell)

		if atlas_coords == TILE_FOREST:
			spawn_tree(cell)

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
