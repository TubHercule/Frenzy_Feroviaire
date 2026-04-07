extends Node2D
class_name Cell_Obj_Container

var cell : Vector2i
var type : Types.CarryType
var nb : int = 0
var max_nb : int = 9
var stack_gap : int = 4
var item_list = []
func _ready() -> void:
	y_sort_enabled = true

#return the number of items that can't be stacked
func add_items(nb_item : int) -> int:
	var dif = 0
	print("nb : ",nb)
	print("nb_item : ",nb_item)
	if (nb+nb_item) > max_nb: #max_exceded

		#print("AT TENTION : contenance case dépassée")
		dif = nb+nb_item-max_nb
		nb = max_nb
	elif (nb+nb_item) < 0:
		nb = 0
		print("ATTENTION : quantité négative")
		dif = nb+nb_item
	else:	#no exces
		nb += nb_item
	display()
	print("dif : ",dif)
	return dif

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
	print("detected")
	var entity = area.get_parent()
	if entity.is_in_group("players") and entity.get_carry_type() == type:
		if entity.get_carry_type() == Types.CarryType.NONE:
			print("ATTENTION : la case contient le type NONE")
			return
		
		entity.try_take()
