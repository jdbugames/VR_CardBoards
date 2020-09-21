//Simple Dual Camera Activator/ Aim down shights script
//Written by Alec Markarian  (http://www.youtube.com/user/misterninjaboy) TUT 5

var Aim : boolean = false; var Cam : GameObject;


function Update () { if(Input.GetMouseButtonDown(1)){ Aim = true; if(Aim == true){ Cam.active = true; } }

if(Input.GetMouseButtonUp(1)){ Aim = true; if(Aim){ Cam.active = false; }
}
}