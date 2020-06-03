extends Spatial

export(float) var ForcePower = 8000.0
export(float) var Stiffness = 0.5
export(float) var Damping = 0.08
export(float) var HoverHeight = 12.0


var ForceToAdd = Vector3()
var Displacement = 0.0
var PrevDisplacement = 0.0
var Speed = 0.0


func _ready():
	$HoverRayCast.cast_to.y = -HoverHeight
	$HoverRayCast.add_exception(get_parent())

func _physics_process(Delta):
	if($HoverRayCast.is_colliding()):
		if(!$HoverCollisionPoint.visible):
			$HoverCollisionPoint.visible = true
		PrevDisplacement = Displacement
		Displacement = ($HoverRayCast.cast_to - global_transform.xform_inv(
													$HoverRayCast.get_collision_point())).length()
		Speed = (Displacement - PrevDisplacement) / Delta
		var SpringForce = get_parent().gravity_scale * get_parent().weight * Stiffness * Displacement
		var DampingForce = Damping * Speed
		ForceToAdd = Vector3.UP * clamp(SpringForce + DampingForce, 0, 50)
		$HoverCollisionPoint.global_transform.origin = $HoverRayCast.get_collision_point()
	else:
		if($HoverCollisionPoint.visible):
			$HoverCollisionPoint.visible = false
		ForceToAdd = Vector3.ZERO
	
	get_parent().add_force(get_parent().global_transform.basis.xform(
					ForceToAdd * Delta * ForcePower), global_transform.origin - get_parent().global_transform.origin)












