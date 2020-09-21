 //Very Simple Animation Setup (v2.0 TUT 5). By; Alec Markarian (http://www.youtube.com/user/misterninjaboy)

//Key down 
     function Update()
{
 if(Input.GetKeyUp("left shift"))
 {
  // Plays the Idle animation - stops all other animations
  animation.Play("Idle", PlayMode.StopAll);
 }
}