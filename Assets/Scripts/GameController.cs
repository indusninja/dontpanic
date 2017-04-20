using UnityEngine;
using System.Collections;

public enum GameState{
	MAIN_MENU,
	PLAYING_LEVEL,
	//INGAME_MENU,
	LEVEL_WIN,
	LEVEL_FAIL
}

public class GameController : MonoBehaviour {
	
	public static bool isHighscoresShown = false;
	public static bool isHighscoresLoadedFromXml = false;
	
	public static bool isCreditsShown = false;
	
	public static GameObject player1;
	public static GameObject player2;
	
	public static GameState currentGameState = GameState.PLAYING_LEVEL;
	
	void Start() {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
			if (player.GetComponent<ControllerSchema>().SchemaKey == InputType.Main) {
				if (player1){
					throw new System.Exception("Two Main Players Detected :(");
				}
				player1 = player;
			} else  if (player.GetComponent<ControllerSchema>().SchemaKey == InputType.Alt) {
				if (player2){
					throw new System.Exception("Two Second Players Detected :(");
				}
				player2 = player;
			}
		}
	}
	
	void Awake() {
		/*if (Application.loadedLevel == 0) {
			currentGameState = GameState.MAIN_MENU;
		}*/
	}
	
	public static void OnPlayerDeath(GameObject obj)
	{
		//print(obj.name + "Died!");
		obj.GetComponent<PlayerController>().die();
	}
	
	public static void LoadNextLevel() {
		int index = Application.loadedLevel + 1;
		if (index > Application.levelCount -1 ) {
			index = 1;
		}
		Application.LoadLevel(index);
		SetPlayingState();
	}
	
	public static void LoadRandomLevel() {
		int index = Random.Range(1,Application.levelCount-1);
		Application.LoadLevel(index);
		SetPlayingState();
	}
	public static void RestartCurrentLevel() {
		Application.LoadLevel(Application.loadedLevel);
		SetPlayingState();
	}
	
	public static void LevelFail(GameObject loser) {
		print("Level lose! - " + loser.gameObject.name + " died.");
		Time.timeScale = 0f;
		currentGameState = GameState.LEVEL_FAIL;
	}
	
	private static void SetPlayingState() {
		currentGameState = GameState.PLAYING_LEVEL;
		Time.timeScale = 1f;
	}
	
	public static void LevelWin() 
	{
		print("Level win!");
		Time.timeScale = 0f;
		currentGameState = GameState.LEVEL_WIN;
	}
	
	public static void showCredits() {
		isCreditsShown = true;
		Time.timeScale = 0f;
	}
	
	public static void closeCredits() {
		isCreditsShown = false;
		if (currentGameState == GameState.PLAYING_LEVEL) {
			Time.timeScale = 1f;
		}
	}
	
	public static void goToMainMenu() {
		Application.LoadLevel(0);
		currentGameState = GameState.MAIN_MENU;
	}
}
