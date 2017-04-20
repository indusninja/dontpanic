using UnityEngine;
using System.Collections;

public class LeaveGame : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
		Debug.Log("Leaving Game");
        Application.Quit();
    }
}
