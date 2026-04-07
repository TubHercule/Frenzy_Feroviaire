extends Node2D
class_name Cell_Obj_Container

var cell : Vector2i
var type : Types.CarryType
var nb : int = 0
var max_nb : int = 9
var stack_gap : int

func add_items(nb_item : int):
	
	if (nb+nb_item) > max_nb:
		nb = max_nb
		print("ATTENTION : contenance case dépassée")
	elif (nb+nb_item) < 0:
		nb = 0
		print("ATTENTION : quantité négative")
	else:
		nb += nb_item
	display()

func display():
	var item_scene = Game_Manager.stackable_items_scenes[self.type]
	print("ouioui baguette ",item_scene)
	for i in nb:
		var item = item_scene.instantiate()
		#item.position = self.position
		self.add_child(item)
