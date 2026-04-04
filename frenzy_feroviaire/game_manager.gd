extends Node2D

@onready var tilemap = $"../TileMap"
@onready var objects_container = $"../obj_container"

var tree_scene = preload("res://scenes/tree.tscn")

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
	var tree = tree_scene.instantiate()

	# position centrée sur la tile
	tree.position = tilemap.map_to_local(cell)+Vector2(0,-8)

	objects_container.add_child(tree)
