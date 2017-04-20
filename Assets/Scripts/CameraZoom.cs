using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	
	private PlayerController[] controllers ;
	public float VerticalOffset = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	void Awake()
	{	
		var gameObjects = GameObject.FindGameObjectsWithTag("Player");
		controllers = new PlayerController[gameObjects.Length];
		int i = 0;
		foreach(GameObject GO in gameObjects)
		{
			controllers[i] = GO.GetComponent<PlayerController>();
			i++;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 CameraPos = Camera.mainCamera.transform.position;
		Vector3 FirstPos, SecondPos; //Controller one and two positions
		FirstPos = controllers[0].transform.position;
		SecondPos = controllers[1].transform.position;
		CameraPos.x = (FirstPos.x + SecondPos.x) * 0.5f ;// avg
		CameraPos.y = (FirstPos.y + SecondPos.y)* 0.5f+VerticalOffset;
		CameraPos.z = -(Mathf.Max(Vector3.Distance(FirstPos,SecondPos)*1.7f,26)/2);
		Camera.mainCamera.transform.position = CameraPos;
	}
}
