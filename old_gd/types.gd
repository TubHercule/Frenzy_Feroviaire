extends Node
#class_name Types

enum CarryType {
	NONE,
	WOOD,
	STONE,
	COAL,
	AXE,
	PICKAXE
}

static func is_tool(type : CarryType):
	return type in [CarryType.AXE, CarryType.PICKAXE]

static func is_resource(type : CarryType):
	return type in [CarryType.WOOD, CarryType.STONE]

enum WorldObjectType {
	NONE,
	TREE,
	ROCK,
	COAL
}
