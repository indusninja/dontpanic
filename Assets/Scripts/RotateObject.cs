using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {
	public float RotateSpeed;
	private float currentRotation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currentRotation = RotateSpeed * Time.fixedDeltaTime;
		transform.RotateAround(new Vector3(0,0,1),currentRotation);
	}
}
