 //Very Simple Animation Setup (v2.0 TUT 5). By; Alec Markarian (http://www.youtube.com/user/misterninjaboy)

//Key down 
     function Update()
{
 if(Input.GetKeyDown("left shift"))
 {
  // Plays the sprint animation - stops all other animations
  animation.Play("sprint", PlayMode.StopAll);
 }
}