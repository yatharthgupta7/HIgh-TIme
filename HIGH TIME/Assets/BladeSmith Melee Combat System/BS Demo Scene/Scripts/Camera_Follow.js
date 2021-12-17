#pragma strict
var Follow : Transform;
var Smoothing : float;
private var TargetPos : Vector3;
var High : float;
var Away : float;
var Main_Cam : Transform;
var CollisionMask : LayerMask;
var CamDistance : float;
var Active : boolean = true;  //Active camera (standard in game)
var Inspect : boolean = false; //Weapon inspecting or taunting camera position
var DebugUp : boolean; //DebugUp sets the Camera over head, pointing down. Used to calibrate weapon ranges.




function FixedUpdate () {

	
	//////////////////////////
	

	if(Active)
	{

	if(!DebugUp)
	{
		TargetPos = Follow.position + (Follow.up * High) - (Follow.forward * Away);
	}
	
	if(DebugUp)
	{	
		TargetPos = Follow.position + (Follow.up * (High+4) - (Follow.forward * 0.3));
	}
		Main_Cam.transform.position = Vector3.Lerp(Main_Cam.transform.position, TargetPos, Time.deltaTime * Smoothing);
		Main_Cam.transform.LookAt(Follow);
			

					
		//Apply rotation					
	var  temp  : Vector3 = Main_Cam.transform.rotation.eulerAngles;
	temp.x = 90.0f;
	Main_Cam.transform.rotation = Quaternion.Euler(temp);





	if(!DebugUp)
	{	

			if(Main_Cam.transform.rotation.eulerAngles.x > 20.0)
			{
			Main_Cam.transform.rotation.eulerAngles.x = 20.0f;
			}
			
			if(Main_Cam.transform.rotation.eulerAngles.x < -5.0)
			{
			Main_Cam.transform.rotation.eulerAngles.x = -5.0f;
			}
	 }
	 
	}

	if(Inspect)
	{

		TargetPos = Follow.position - (Follow.right *2)  + (Follow.forward * 2.3);
		Main_Cam.transform.position = Vector3.Lerp(Main_Cam.transform.position, TargetPos, Time.deltaTime * Smoothing);
		
Main_Cam.transform.LookAt(Follow);
	}
	
}	







