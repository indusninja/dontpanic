using UnityEngine;
using System.Collections;

public class TrapDoorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			//print("Player stepped on trap door! " + transform.parent.GetComponent<Rigidbody>().isKinematic);
			transform.parent.GetComponent<Rigidbody>().isKinematic = false;
			transform.parent.GetComponent<Rigidbody>().WakeUp();
		}
	}
}
