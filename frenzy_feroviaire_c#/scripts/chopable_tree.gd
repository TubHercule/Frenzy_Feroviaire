extends Decor
class_name Chopable_Tree


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	super()
	self.dropped_item_type = Types.CarryType.WOOD
	self.decor_type = Types.WorldObjectType.TREE
	# print(y_sort_enabled)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
