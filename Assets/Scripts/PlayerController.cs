using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour {
	
	public float NormalSpeed = 3f;
	public float RunSpeed = 6f;
	public float JumpFactor = 1.7f;
	public float Gravity = 20f;
	public float MaxFallVelocity = 20f;
	
	private float horizontalMovement = 0f;
	private float verticalMovement = 0f;
	private bool grounded = false;
	private bool crouched = false;
	private bool prevcrouch = false;
	private bool deleteMode = false;
	private bool quitMode = false;
	private bool buttonHasBeenReleased = true;
	
	private int lvlTabShown = 1;
	
	private string lastKeyTyped; //Dont use it assuming it always contains the last key typed. It does not.
	
	private string[,,] highscores;
	
	private Highscores hs;
	
	private CollisionFlags flags;
	private CharacterController controller;
	
	public Texture2D ChargeBarTexture;
	public Texture2D HighscoreBackground; //= Resources.Load("HighscoreBackground.jpg"); //Add a pretty background later.
	
	
	// Moving platform support.
	private Transform activePlatform;
	private Vector3 activeLocalPlatformPoint;
	private Vector3 activeGlobalPlatformPoint;
	private Vector3 lastPlatformVelocity;
	
	private float currentBeltSpeedChange;
	
	private PlayerEnergy playerEnergy;
	
	private bool WalkedThisFrame = false;
	
	private float targetRotation = 0f;
	
	void Awake()
	{
		playerEnergy = GetComponent<PlayerEnergy>();
		if(animation==null)
			print("No animation found on parent level!");
		else
		{
			print("found animation on parent: " + animation.name);
			SetupAnimation(animation);
		}
		
		if(GetComponentInChildren<Animation>()==null)
			print("No animation found on children level!");
		else
		{
			print("found animation on child: " + GetComponentInChildren<Animation>().name);
			SetupAnimation(GetComponentInChildren<Animation>());
		}
	}
	
	void SetupAnimation(Animation animObj)
	{
		animObj.wrapMode = WrapMode.Loop;
		animObj["idle"].layer = -1;
		animObj["walk"].layer = -1;
		animObj["walkwithball"].layer = -1;
		animObj["idlewithball"].layer = -1;
		animObj["falling"].layer = -1;
		animObj.SyncLayer(-1);
		
		animObj["jump"].layer = 10;
		animObj["jump"].wrapMode = WrapMode.Once;
		animObj["stopping"].layer = 10;
		animObj["stopping"].wrapMode = WrapMode.Once;
		animObj["startwalk"].layer = 10;
		animObj["startwalk"].wrapMode = WrapMode.Once;
		animObj["stopwithball"].layer = 10;
		animObj["stopwithball"].wrapMode = WrapMode.Once;
		animObj["landing"].layer = 10;
		animObj["landing"].wrapMode = WrapMode.Once;
		animObj.SyncLayer(10);
		
		animObj.Stop();
		animObj.Play("idle");
	}
	
	// FixedUpdate used for physics calculations
	void FixedUpdate () 
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * 7f), transform.eulerAngles.z);
		//transform.eulerAngles.y -= (transform.eulerAngles.y - targetRotation); 
		/*if(!grounded && !GetComponentInChildren<Animation>().isPlaying)
		{
			GetComponentInChildren<Animation>().CrossFade("falling");
		}
		else
		{
			GetComponentInChildren<Animation>().CrossFade("idle");
		}*/
		
		// Moving platform support
		if (activePlatform != null) 
		{
			var newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
			var moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
			transform.position = transform.position + moveDistance;
			lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
		}
		else
		{
			lastPlatformVelocity = Vector3.zero;	
		}
	
		activePlatform = null;
		
		if(verticalMovement > -1 * MaxFallVelocity)
			verticalMovement -= Gravity * Time.deltaTime;

		controller = GetComponent<CharacterController>();
		flags = controller.Move(new Vector3(horizontalMovement, verticalMovement, 0) * Time.deltaTime);
		
		grounded = (flags & CollisionFlags.CollidedBelow)!=0;
		
		if(crouched && !prevcrouch)
		{
			controller.transform.localScale = new Vector3(controller.transform.localScale.x, 0.75f, controller.transform.localScale.z);
		}
		if(!crouched && prevcrouch) 
		{
			controller.transform.localScale = new Vector3(controller.transform.localScale.x, 1f, controller.transform.localScale.z);
			controller.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y + 0.25f, controller.transform.position.z);
		}
		
		// Moving platforms support
		if (activePlatform != null) 
		{
			activeGlobalPlatformPoint = transform.position;
			activeLocalPlatformPoint = activePlatform.InverseTransformPoint (transform.position);
		}

		prevcrouch = crouched;
		
		
		if(WalkedThisFrame == true)
		{
			playerEnergy.DoWalk();
			WalkedThisFrame = false;
		}	
		
		if(!playerEnergy.IdleDrain() || !playerEnergy.CanWalk())
			die();
	}
	
	public void AcceptInput(string key, float val, bool state)
	{
		switch(key)
		{
			case "horizontal":
				if(grounded)
				{
					if(val != 0)
						WalkedThisFrame = true;
					if(val > 0)
					{
						horizontalMovement = val * (NormalSpeed + currentBeltSpeedChange);
						//targetRotation = -90f;
					}
					else if(val < 0)
					{
						horizontalMovement = val * (NormalSpeed - currentBeltSpeedChange);
						//targetRotation = 90f;
					}
					else if(currentBeltSpeedChange != 0)
						horizontalMovement= currentBeltSpeedChange;
					else 
						horizontalMovement = 0;
					if(val != 0)
					{
						if(horizontalMovement > 0)
							targetRotation = 0f;
						else if (horizontalMovement < 0)
							targetRotation = 180f;
						GetComponentInChildren<Animation>().CrossFade("walk");
						//print(key + " : walking");
					}
					else
						GetComponentInChildren<Animation>().CrossFade("idle");
				}
				break;
			case "jump":
				if(grounded && playerEnergy.Jump())
				{
					grounded = false;
					verticalMovement = (NormalSpeed * JumpFactor) + lastPlatformVelocity.y;
					crouched = state;
					GetComponentInChildren<Animation>().CrossFade("jump");
					//print(key + " : jumping");
				}
				break;
			case "crouch":
				if(grounded)
				{
					crouched = state;
				}
				break;
			default:
				GetComponentInChildren<Animation>().CrossFade("idle");
				//print(key + " : idling");
				break;
		}
	}
	
	bool IsTouchingCeiling () 
	{
		return (flags & CollisionFlags.CollidedAbove) != 0;
	}
	
	void ResetMotion()
	{
		verticalMovement = 0f;
		horizontalMovement = 0f;
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.moveDirection.y > 0.01) 
			return;
		
		// Make sure we are really standing on a straight platform
		// Not on the underside of one and not falling down from it either!
		if (hit.moveDirection.y < -0.9 && hit.normal.y > 0.9) {
			activePlatform = hit.collider.transform;	
		}
	}
	
	void OnTriggerEnter(Collider other)
    {
        var beltTrigger = other.GetComponent<BeltTrigger>();  
        if (beltTrigger != null)
		{
			currentBeltSpeedChange = beltTrigger.SpeedChange;
		}
    }
	
	void OnTriggerExit(Collider other)
	{
		currentBeltSpeedChange = 0;
	}
	
	//A method for respawning the player, with initial values when (s)he dies.
	//It was preferred about reseting the gameobject, as it could be possible
	//in a later stage that the player should possess some persistent information
	//(Potential powerups, keys, etc)
	public void RespawnPlayer(){
		//All this method currently does it just reset the charge level..
		playerEnergy.CurrentCharge = playerEnergy.MaxCharge;
		//acctually respawns the palyer in the last checkpoint
		if(GetComponent<ControllerSchema>().SchemaKey == InputType.Main){
			transform.position = Checkpoint.lastCheckpointCoordinatesMain;	
		}
		else if(GetComponent<ControllerSchema>().SchemaKey == InputType.Alt){
			transform.position = Checkpoint.lastCheckpointCoordinatesAlt;
		}
	}
	
	//Kills the player and handles whatever needs to be handled :P
	public void die() {
		RespawnPlayer();
	}
	
	void OnGUI(){
		if(!GameController.isHighscoresShown){
			if(deleteMode){
				GUI.Label(new Rect(10,Screen.height - 65,200, 20), "Reset Highscores?");
				GUI.Label(new Rect(10,Screen.height - 45,200, 20), "Yes(Y)");
				GUI.Label(new Rect(10,Screen.height - 25,200, 20), "No(N)");
				
				if(Input.GetKeyDown("y")){
					deleteMode = false;
					Highscores.ResetHighScores();
				}
				else if(Input.GetKeyDown("n")){
					deleteMode = false;
				}
			}
			else if(quitMode){
				GUI.Label(new Rect(10,Screen.height - 65,200, 20), "Leave Game?");
				GUI.Label(new Rect(10,Screen.height - 45,200, 20), "Yes(Y)");
				GUI.Label(new Rect(10,Screen.height - 25,200, 20), "No(N)");
				
				if(Input.GetKeyDown("y")){
					Debug.Log("Quitting");
					Application.Quit();
				}
				else if(Input.GetKeyDown("n")){
					quitMode = false;
				}
			}
			else{
				GUI.Label(new Rect(10,Screen.height - 25,200, 20), "Total Highscores(T)");
				GUI.Label(new Rect(10,Screen.height - 45,200, 20), "Level Highscores(L)");
				GUI.Label(new Rect(10,Screen.height - 65,200, 20), "Reset Highscores(R)");
				GUI.Label(new Rect(Screen.width-95, Screen.height - 25, 200, 20), "Quit Game(Q)");				
				
				if(Input.GetKeyDown("l")){
					GameController.isHighscoresShown = true;
					lastKeyTyped = "l";
				}
				else if(Input.GetKeyDown("t")){
					GameController.isHighscoresShown = true;
					lastKeyTyped = "t";
				}
				else if(Input.GetKeyDown("r")){
					deleteMode = true;
				}
				else if(Input.GetKeyDown("q")){
					quitMode = true;
				}
			}
		}
		else{
			if(lastKeyTyped == "l"){
				if(GameController.isHighscoresLoadedFromXml == false){				
					highscores = Highscores.GetLevelHighscores();
					GameController.isHighscoresLoadedFromXml = true;
				}
				if(!Input.anyKey){
					buttonHasBeenReleased = true;
				}
				//This one is just for testing purposes... adds some values to the highscores
				else if(Input.GetKeyDown("o") && buttonHasBeenReleased){ //p for POWER OOooVER..WHELMNING..(<--Sc2 archon quote<3).. enough fun
					Highscores.AddScore("Jonas", 10, 1);
					Highscores.AddScore("Leo", 7, 3);
					Highscores.AddScore("Prakash", 4, 3);
					Highscores.AddScore("Mikkel", 9, 3);
					Highscores.AddScore("Peter", 7, 0);
					Highscores.AddScore("Lasse", 8, 0);
					buttonHasBeenReleased = false;
				}
				else if(Input.GetKeyDown("p") && buttonHasBeenReleased){
					if(lvlTabShown > 1){
						lvlTabShown--;
					}	
					buttonHasBeenReleased = false;
				}
				else if(Input.GetKeyDown("n") && buttonHasBeenReleased){
					if(lvlTabShown < Highscores.amountOfLevels){
						lvlTabShown++;
					}
					buttonHasBeenReleased = false;
				}
				Debug.Log(highscores[1,0,0]);
				DrawHighscores("Level Highscores", highscores);
			}
			else if(lastKeyTyped == "t"){
				if(GameController.isHighscoresLoadedFromXml == false){				
					highscores = Highscores.GetTotalHighscore();
					GameController.isHighscoresLoadedFromXml = true;
					lvlTabShown = 1;
				}
				DrawHighscores("Total Highscores", highscores);
			}
			
			
			if(Input.GetKeyDown("q")){
				GameController.isHighscoresShown = false;
				GameController.isHighscoresLoadedFromXml = false;
			}
		}
	}
	
	private void DrawHighscores(string highscoreTitle, string[,,] scores){
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), highscoreTitle); //add custom background
	
		//1
		GUI.Box(new Rect(10, 25, 20, 25), "1.");
		GUI.Box(new Rect(30, 25, 200, 25), scores[(lvlTabShown-1), 0, 0]);
		GUI.Box(new Rect(230, 25, 200, 25), scores[(lvlTabShown-1), 0, 1]);
		
		//2
		GUI.Box(new Rect(10, 55, 20, 25), "2.");
		GUI.Box(new Rect(30, 55, 200, 25), scores[lvlTabShown-1, 1, 0]);
		GUI.Box(new Rect(230, 55, 200, 25), scores[lvlTabShown-1, 1, 1]);
		
		//3
		GUI.Box(new Rect(10, 85, 20, 25), "3.");
		GUI.Box(new Rect(30, 85, 200, 25), scores[lvlTabShown-1, 2, 0]);
		GUI.Box(new Rect(230, 85, 200, 25), scores[lvlTabShown-1, 2, 1]);
		
		//4
		GUI.Box(new Rect(10, 115, 20, 25), "4.");
		GUI.Box(new Rect(30, 115, 200, 25), scores[lvlTabShown-1, 3, 0]);
		GUI.Box(new Rect(230, 115, 200, 25), scores[lvlTabShown-1, 3, 1]);
		
		//5
		GUI.Box(new Rect(10, 145, 20, 25), "5.");
		GUI.Box(new Rect(30, 145, 200, 25), scores[lvlTabShown-1, 4, 0]);
		GUI.Box(new Rect(230, 145, 200, 25), scores[lvlTabShown-1, 4, 1]);
		
		//6
		GUI.Box(new Rect(Screen.width-430, 25, 20, 25), "6.");
		GUI.Box(new Rect(Screen.width-410, 25, 200, 25), scores[lvlTabShown-1, 5, 0]);
		GUI.Box(new Rect(Screen.width-210, 25, 200, 25), scores[lvlTabShown-1, 5, 1]);
		
		//7
		GUI.Box(new Rect(Screen.width-430, 55, 20, 25), "7.");
		GUI.Box(new Rect(Screen.width-410, 55, 200, 25), scores[lvlTabShown-1, 6, 0]);
		GUI.Box(new Rect(Screen.width-210, 55, 200, 25), scores[lvlTabShown-1, 6, 1]);
		
		//8
		GUI.Box(new Rect(Screen.width-430, 85, 20, 25), "8.");
		GUI.Box(new Rect(Screen.width-410, 85, 200, 25), scores[lvlTabShown-1, 7, 0]);
		GUI.Box(new Rect(Screen.width-210, 85, 200, 25), scores[lvlTabShown-1, 7, 1]);
		
		//9
		GUI.Box(new Rect(Screen.width-430, 115, 20, 25), "9.");
		GUI.Box(new Rect(Screen.width-410, 115, 200, 25), scores[lvlTabShown-1, 8, 0]);
		GUI.Box(new Rect(Screen.width-210, 115, 200, 25), scores[lvlTabShown-1, 8, 1]);
		
		//10
		GUI.Box(new Rect(Screen.width-430, 145, 20, 25), "10.");
		GUI.Box(new Rect(Screen.width-410, 145, 200, 25), scores[lvlTabShown-1, 9, 0]);
		GUI.Box(new Rect(Screen.width-210, 145, 200, 25), scores[lvlTabShown-1, 9, 1]);
		
		if(lastKeyTyped == "l"){
			GUI.Label(new Rect(10,Screen.height - 85,200, 20), "Currently Showing Level: " + lvlTabShown);
			GUI.Label(new Rect(10,Screen.height - 65,300, 20), "Show Previous Level's Highscores(P)");
			GUI.Label(new Rect(10,Screen.height - 45,200, 20), "Show Next Level's Highscores(N)");
		}
		
		GUI.Label(new Rect(10,Screen.height - 25,200, 20), "Exit Highscores(Q)");
	}
}
