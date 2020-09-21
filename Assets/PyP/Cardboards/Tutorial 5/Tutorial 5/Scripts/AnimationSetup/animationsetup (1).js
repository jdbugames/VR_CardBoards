 //Very Simple Animation Setup (v2.0 TUT 5). By; Alec Markarian (http://www.youtube.com/user/misterninjaboy)

//Key down 
     function Update()
{
 if(Input.GetMouseButtonDown(0))
 {
  // Plays the fire1 animation - stops all other animations
  animation.Play("fire1", PlayMode.StopAll);
 }
}