class_name Types

enum CarryType {
	NONE,
	WOOD,
	STONE,
	AXE,
	PICKAXE
}

static func is_tool(type : CarryType):
	return type in [CarryType.AXE, CarryType.PICKAXE]

func is_resource(type : CarryType):
	return type in [CarryType.WOOD, CarryType.STONE]
