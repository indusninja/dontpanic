using UnityEngine;
using System.Collections;

public class CircularMotion : MonoBehaviour {
	
	public float speed = 2f;
	private float radius;
	private GameObject center;
	private float acc;
	// Use this for initialization
	void Start () {
		center = transform.parent.gameObject;
		Vector3 distance = transform.position - center.transform.position;
		radius = distance.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
		//y fixed.
		
		acc += speed * Time.deltaTime;
		
		if (acc >= 1){
			acc = 0;
		}
		SetPercentage(acc);
	}
	
	void SetPercentage(float p) {
		Vector3 pos = Vector3.zero;
		pos.x = radius * Mathf.Cos(p*360*Mathf.Deg2Rad);
		pos.z = radius * Mathf.Sin(p*360*Mathf.Deg2Rad);
		pos += center.transform.position;
		pos.y = transform.position.y;
		transform.position = pos;
	}
}
