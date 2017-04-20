using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
		GameController.LoadNextLevel();
    }
}
