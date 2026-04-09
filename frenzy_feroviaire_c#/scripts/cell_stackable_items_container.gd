extends Cell_Obj_Container
class_name Cell_Stackable_Items_Container

var stack_gap : int = 2

func _ready() -> void:
	super()

func init(_type:Types.CarryType):
	self.type = _type


#@override
func display():
	var item_scene = Game_Manager.stackable_items_scenes[self.type]
	var dif = nb-item_list.size()
	var original_size = item_list.size()
	print("dif : ",dif)
	if (dif > 0):
		for i in range(dif):
			var item = item_scene.instantiate()
			item.position += Vector2(0,-(i+original_size)*stack_gap)
			item_list.append(item)
			self.add_child(item)
	else:
		for i in range(-dif):
			print("obj removed")
			item_list.pop_back().queue_free()


func _on_area_2d_area_entered(area: Area2D) -> void:
	detect_entity(area)
