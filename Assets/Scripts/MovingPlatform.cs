using UnityEngine;
using System.Collections;

public class MovingPlatform : MovableTarget {
	public float WaitOnEdges = 2f; //sec
	
	//private float currentP = 0f;
	private float waiting = 0f;	
	// Use this for initialization
	void Start () {
		initialPosition = this.transform.position;
		stand = this.transform.Find("Stand").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (waiting > 0) {
			waiting -= Time.deltaTime;
		} else {
			Vector3 distance = destination.position - initialPosition;
			float stepP = speed / distance.magnitude  ;
			stepP *= Time.deltaTime;
	
			currentP += stepP;
			if (currentP >= 1 || currentP <= 0 ){
				if(currentP >= 1) {
					currentP = 1;
				} else {
					currentP = 0;
				}
				speed *= -1;
				waiting = WaitOnEdges;
			}
			
			setProgress(currentP);
		}
	}
}
