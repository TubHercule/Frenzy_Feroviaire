extends Node2D
class_name Cell_Obj_Container

var cell : Vector2i
var type : Types.CarryType
var nb : int = 0
@export var max_nb : int
var item_list = []
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	y_sort_enabled = true

func is_full():
	return max_nb == nb

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

#will be override by child class
func display():
	pass

func set_max_nb(_max_nb):
	if max_nb > 0:
		max_nb = _max_nb
	else:
		print("ATTENTION : you tried to set max_nb<0")

func detect_entity(area : Area2D):
	print("detected")
	var entity = area.get_parent()
	if entity.is_in_group("players") and entity.get_carry_type() == type:
		if entity.get_carry_type() == Types.CarryType.NONE:
			print("ATTENTION : la case contient le type NONE")
			return
		
		entity.try_take()
