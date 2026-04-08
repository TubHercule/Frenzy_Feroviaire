extends Cell_Obj_Container
class_name Cell_Tools_Container

var can_destroy : Array[Types.WorldObjectType] = []
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	super()

func init(_type:Types.CarryType, _can_destroy : Array[Types.WorldObjectType]) -> void:
	self.type = _type
	self.can_destroy = _can_destroy
	
#@override
func set_max_nb(_max_nb):
	print("ATTENTION : vous essayez de modifier la max_nb d'un outil")

#@override
func display():
	var item_scene = Game_Manager.stackable_items_scenes[self.type]
	#var dif = nb-item_list.size()
	#print("dif : ",dif)
	var item = item_scene.instantiate()
	item_list.append(item)
	self.add_child(item)


func _on_area_2d_area_entered(area: Area2D) -> void:
	detect_entity(area)
