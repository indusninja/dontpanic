using UnityEngine;
using System.Collections;

public class DeathlyObstacle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {	
			print("[DeathlyObstacle] Player Touched and will die!!!");
			other.GetComponent<PlayerController>().die();
		}
	}
}
