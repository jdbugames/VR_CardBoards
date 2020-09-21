//Bullet Damage (To be used with PrefabShooting.js (Watch tut 5)
//Written by Alec Markarian (http://www.youtube.com/user/misterninjaboy)
#pragma strict

var Dammage = 100;

function OnCollisionEnter (info : Collision)
{
	info.transform.SendMessage("ApplyDammage", Dammage, SendMessageOptions.DontRequireReceiver);
}