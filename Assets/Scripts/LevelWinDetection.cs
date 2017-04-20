using UnityEngine;
using System.Collections;

public class LevelWinDetection : MonoBehaviour {
	
	GameObject[] PlayerObjects;
	float PlayerDistance = 1.5f;
	bool recheck = false;
	
	// Use this for initialization
	void Start () {
		PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(recheck)
		{
			print("Still rechecking!!!");
			if(CheckLevelCondition())
			{
				recheck = false;
				GameController.LevelWin();
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			recheck = true;
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			recheck = false;
		}
	}
	
	bool CheckLevelCondition()
	{
		if(PlayerObjects.Length >= 2)
		{
			float distanceSq = (PlayerObjects[0].transform.position - PlayerObjects[1].transform.position).sqrMagnitude;
			bool ballAquired = PlayerObjects[0].GetComponent<PlayerCarry>().IsCarrying() || 
				PlayerObjects[1].GetComponent<PlayerCarry>().IsCarrying();
			if(distanceSq <= PlayerDistance * PlayerDistance && ballAquired)
			{
				print("Level finished Successfully!");
				return true;
			}
			else
			{
				print("Level finish Failed!");
				return false;
			}
		}
		return false;
	}
}
