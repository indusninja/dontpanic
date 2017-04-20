using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class CameraTrigger : MonoBehaviour {
    private  PlayerController controller;

    // Use this for initialization
    void Start () {
        
    }

    void OnTriggerEnter(Collider other)
    {
        controller = other.GetComponent<PlayerController>();  
        if (controller == null)
        { Debug.Log("Null"); }
        else 
            Debug.Log("Not null!");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
    }
	
    // Update is called once per frame
    void Update () {
	       
    }
}
