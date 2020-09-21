/**
*	Script written by Jackson Medeiros (BlackStorm)
*	Do not remove this
**/

var hitPoints = 100.0;
var prefab : Transform;

function ApplyDamage (damage : float) {

	if (hitPoints <= 0.0)
		return;

	hitPoints -= damage;
	if (hitPoints <= 0.0)
	{
		Detonate();
	}
}

function Detonate() {

	if (prefab)
		Instantiate (prefab, transform.position, transform.rotation);
		
	Destroy(gameObject);
}

@script RequireComponent (Rigidbody)