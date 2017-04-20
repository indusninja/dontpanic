using UnityEngine;
using System.Collections;

public class LiftSensor : MonoBehaviour {
	private Lift lift;
	// Use this for initialization
	void Start () {
		lift = transform.parent.parent.GetComponent<Lift>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			lift.goToDestination();
			print("[LiftSensor] Player detected");
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			lift.goBack();
			print("[LiftSensor] Player not detected anymore");
		}
	}
}
