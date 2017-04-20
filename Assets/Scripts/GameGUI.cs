using UnityEngine;
using System.Collections;

public class GameGUI : MonoBehaviour {
	
	public Texture healthBackground;
	public Texture healthBackgroundRight;
	public Texture health;
	public Texture healthRight;
	public Texture overcharge;
	public Texture overchargeRight;
	public Texture avatar1;
	public Texture avatar2;
	
	public Texture credit;
	public Texture mainMenu;
	
	private Rect winScreenRect = new Rect(20,20,150,320);
	private Rect mainMenuScreenRect = new Rect(20,20,150,320);
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		if (!GameController.isCreditsShown) {
			switch(GameController.currentGameState)
			{
				case GameState.PLAYING_LEVEL:
					showPlayersHealth();
					break;
				case GameState.LEVEL_WIN:
					showWinScreen();
					break;
				case GameState.LEVEL_FAIL:
					showFailScreen();
					break;
				case GameState.MAIN_MENU:
					showMainMenu();
					break;
				default:
					break;
			}
		} else if (GameController.isCreditsShown) {
			showCredits();
		}
    }
	
	void showFailScreen() {
		winScreenRect.x = (Screen.width - winScreenRect.width)/2;
		winScreenRect.y = (Screen.height - winScreenRect.height)/2;
		winScreenRect = GUI.Window(1, winScreenRect, failWindow, "Level Failed");
	}
	
	void showWinScreen() {
		winScreenRect.x = (Screen.width - winScreenRect.width)/2;
		winScreenRect.y = (Screen.height - winScreenRect.height)/2;
		winScreenRect = GUI.Window(0, winScreenRect, winWindow, "Level Complete");
	}
	
	void winWindow(int windowID) {
		const int windowPadding = 10;
		float componentsWidth = winScreenRect.width - windowPadding - windowPadding;
		
	  	GUI.Label(new Rect(windowPadding, 20, componentsWidth, 40), "You Win!");
		
		if (GUI.Button(new Rect(windowPadding, 70, componentsWidth, 40), "Proceed")) {
        	GameController.LoadRandomLevel();
		}
		
		if (GUI.Button(new Rect(windowPadding, 120, componentsWidth, 40), "Play Again")) {
        	GameController.RestartCurrentLevel();
		}
		
		if (GUI.Button(new Rect(windowPadding, 170, componentsWidth, 40), "Credits")) {
        	GameController.showCredits();
		}
		
		if (GUI.Button(new Rect(windowPadding, 220, componentsWidth, 40), "Main Menu")) {
        	GameController.goToMainMenu();
		}
		
		if (GUI.Button(new Rect(windowPadding, 270, componentsWidth, 40), "Quit")) {
			//TODO: back to main menu
        	Application.Quit();
		}
	}
	
	void failWindow(int windowID) {
		const int windowPadding = 10;
		float componentsWidth = winScreenRect.width - windowPadding - windowPadding;
		
	  	GUI.Label(new Rect(windowPadding, 20, componentsWidth, 40), "You Failed!");
		
		if (GUI.Button(new Rect(windowPadding, 70, componentsWidth, 40), "Play Again")) {
        	GameController.RestartCurrentLevel();
		}
		
		if (GUI.Button(new Rect(windowPadding, 120, componentsWidth, 40), "Quit")) {
			//TODO: back to main menu
        	Application.Quit();
		}
	}
	
	void showCredits() {
		float x = (Screen.width - credit.width)/2;
		float y = (Screen.height - credit.height)/2;
		if (GUI.Button(new Rect(x,y,credit.width, credit.height),credit)){
			GameController.closeCredits();
		}
	}
	
	void showMainMenu() {
		mainMenuScreenRect.width = mainMenu.width;
		mainMenuScreenRect.height = mainMenu.height;
		mainMenuScreenRect.x = (Screen.width - mainMenuScreenRect.width)/2;
		mainMenuScreenRect.y = (Screen.height - mainMenuScreenRect.height)/2;
		mainMenuScreenRect = GUI.Window(3, mainMenuScreenRect, mainMenuWindow, "Menu");
	}
	
	void mainMenuWindow(int windowID) {
		const float buttonHeight = 40;
		const float buttonWidth = 150;
		float buttonY = mainMenu.height - buttonHeight - 20f;
		
		GUI.DrawTexture(new Rect(0,0,mainMenu.width,mainMenu.height), mainMenu, ScaleMode.ScaleAndCrop,true,0f);
		
		if (GUI.Button(new Rect(20,buttonY,buttonWidth,buttonHeight),"Quit")) {
			Application.Quit();
		}
		if (GUI.Button(new Rect((mainMenu.width - buttonWidth)/2,buttonY,buttonWidth,buttonHeight),"Credits")) {
			GameController.showCredits();
		}
		if (GUI.Button(new Rect(mainMenu.width - buttonWidth - 20 ,buttonY,buttonWidth,buttonHeight),"Play")) {
			GameController.LoadRandomLevel();
		}
		
	}
	
	void showPlayersHealth() {
		const int healthBarWidth = 278;
        //GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "This is a title");
		var player1 = GameController.player1;
		var player2 = GameController.player2;
		plotHealth(player1.GetComponent<PlayerEnergy>(), 10f, 10f);
		plotHealthRight(player2.GetComponent<PlayerEnergy>(), Screen.width - healthBarWidth - 75f - 10f , 10f);
		//print(Screen.width);
		//print(Screen.width - healthBarWidth - 10f - 300f);
	}
	
	void plotHealth(PlayerEnergy playerEnergy, float x, float y) {
		const int paddingTop = 6;
		const int paddingLeft = 5;
		const int innerWidth = 256;
		const int innerHeight = 37;
		const int avatarPadding = 75;
		
		float totalWidth = healthBackground.width;
		float totalheight = healthBackground.height;
		//print(healthBackground.width);
		
		//avatar1
		GUI.DrawTexture(new Rect(x, y, avatar1.width,  avatar1.height),  		avatar1,	ScaleMode.ScaleAndCrop, true, 0.0F);
		
		float healthWidth = paddingLeft + ((playerEnergy.CurrentCharge / playerEnergy.MaxCharge) * (innerWidth));
		float heatWidth   = /*paddingLeft +*/ ((playerEnergy.CurrentHeat   / playerEnergy.MaxHeat)   * (innerWidth));
		
		GUI.DrawTexture(new Rect(x+avatarPadding, y, totalWidth,  totalheight),  		healthBackground,	ScaleMode.ScaleAndCrop, true, 0.0F);
		
		GUI.BeginGroup(new Rect(x + avatarPadding+ paddingLeft, y, healthWidth, health.height));
		GUI.DrawTexture(new Rect(-paddingLeft, 0, health.width, health.height), 	health,			ScaleMode.ScaleAndCrop, true, 0.0F);
		GUI.EndGroup();
		
		GUI.BeginGroup(new Rect(x + avatarPadding + paddingLeft, y, heatWidth, overcharge.height));
		GUI.DrawTexture(new Rect(-paddingLeft, 0, overcharge.width, overcharge.height), overcharge, 	ScaleMode.ScaleAndCrop, true, 0.0F);
		GUI.EndGroup();
		
		//GUI.DrawTexture(new Rect(x, y, heatWidth,   overcharge.height), overcharge, 	ScaleMode.ScaleAndCrop, true, 0.0F);
	}
	
	void plotHealthRight(PlayerEnergy playerEnergy, float x, float y) {
		const int paddingTop = 7;
		const int paddingLeft = 16;
		const int innerWidth = 256;
		const int innerHeight = 37;
		
		float totalWidth = healthBackgroundRight.width;
		float totalheight = healthBackgroundRight.height;
		//print(healthBackground.width);
		
		GUI.DrawTexture(new Rect(x + 278, y, avatar1.width,  avatar1.height),  		avatar2,	ScaleMode.ScaleAndCrop, true, 0.0F);
		
		float healthWidth = /*paddingLeft +*/ ((playerEnergy.CurrentCharge / playerEnergy.MaxCharge) * (innerWidth));
		float heatWidth   = /*paddingLeft +*/ ((playerEnergy.CurrentHeat   / playerEnergy.MaxHeat)   * (innerWidth));
		
		GUI.DrawTexture(new Rect(x, y, totalWidth,  totalheight),  		healthBackgroundRight,	ScaleMode.ScaleAndCrop, true, 0.0F);
		
		GUI.BeginGroup(new Rect(x + paddingLeft + (innerWidth - healthWidth), y, healthWidth, healthRight.height));
		GUI.DrawTexture(new Rect(-paddingLeft, 0, healthRight.width, healthRight.height), 	healthRight,			ScaleMode.ScaleAndCrop, true, 0.0F);
		GUI.EndGroup();
		
		GUI.BeginGroup(new Rect(x + paddingLeft + (innerWidth - heatWidth), y, heatWidth, overchargeRight.height));
		GUI.DrawTexture(new Rect(-paddingLeft, 0, overchargeRight.width, overchargeRight.height), overchargeRight, 	ScaleMode.ScaleAndCrop, true, 0.0F);
		GUI.EndGroup();
		
		//GUI.DrawTexture(new Rect(x, y, heatWidth,   overcharge.height), overcharge, 	ScaleMode.ScaleAndCrop, true, 0.0F);
	}
}
