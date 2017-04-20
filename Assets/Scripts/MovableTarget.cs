using UnityEngine;
using System.Collections;

public class MovableTarget : MonoBehaviour {

	public float speed = 1f;
	public Transform destination;
	
	protected GameObject stand;
	protected Vector3 initialPosition;
	private bool goToTarget = false;
	protected float currentP = 0f;

	// Use this for initialization
	void Start () {
		initialPosition = this.transform.position;
		stand = this.transform.Find("Stand").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (goToTarget && speed != 0) {
			
			if(!GetComponent<AudioSource>().isPlaying)
			{
				print("playing elevator sound!");
				GetComponent<AudioSource>().Play();
			}
			///
			Vector3 distance = destination.position - initialPosition;
			float stepP = speed / distance.magnitude;
			stepP *= Time.deltaTime;
			currentP += stepP;
			if (currentP >= 1 || currentP <= 0 ){
				if(currentP >= 1) {
					currentP = 1;
				} else {
					currentP = 0;
				}
				goToTarget = false;
			}
			setProgress(currentP);
		}
		else
		{
			GetComponent<AudioSource>().Stop();
		}
	}
	
	public void setProgress(float p) {
		/*print(stand);
		print(initialPosition);
		print(destination);*/
		stand.transform.position = Vector3.Lerp(initialPosition,destination.position,p);
	}
	
	public void goToDestination() {
		goToTarget = true;
		speed = Mathf.Abs(speed);
	}
	
	public void goBack() {
		goToTarget = true;
		speed = -Mathf.Abs(speed);
	}
}
