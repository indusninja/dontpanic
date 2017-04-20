using UnityEngine;
using System.Collections;

public class ZPlaneComponent : MonoBehaviour {

	void OnTriggerEnter(Collider collisionObject)
	{
		if(collisionObject.gameObject.tag=="Player")
		{
			GameController.OnPlayerDeath(collisionObject.gameObject);
			//print("Death Collision Detected");
		}
		else
		{
			Destroy(collisionObject.gameObject);
		}
	}
}
