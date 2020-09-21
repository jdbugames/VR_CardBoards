@script RequireComponent(AudioSource)
 
var front : AudioClip;

var back: AudioClip;

function Update(){
 
if (Input.GetButtonDown("Fire1")) { audio.PlayOneShot(front, 1.0); }
 
}