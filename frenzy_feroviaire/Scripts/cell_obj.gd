extends RefCounted
class_name Cell_Obj

var type
var nb : int

func _init(_customType = null, _value = 0):
	type = _customType
	nb = _value
