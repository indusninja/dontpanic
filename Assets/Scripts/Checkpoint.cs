using UnityEngine;
using System.Collections;

//This script assumes the player object has the tag "Player"
public class Checkpoint : MonoBehaviour {

	public static Vector3 lastCheckpointCoordinatesMain = Vector3.zero;
	public static Vector3 lastCheckpointCoordinatesAlt = Vector3.zero;
	
	//When something enters the checkpoint. Save this checkpoint as being the latest
	void OnTriggerEnter(Collider other) {
		if(this.tag == "Checkpoint"){
        	if(other.tag ==	"Player") {
				//using the y coordinate of the player, to make sure  he does not fall through the floor, 
				//if the checkpoint is placed retarded.
				if(other.GetComponent<ControllerSchema>().SchemaKey == InputType.Main){
					lastCheckpointCoordinatesMain = new Vector3(this.transform.position.x, other.transform.position.y, this.transform.position.z);
				}
				if(other.GetComponent<ControllerSchema>().SchemaKey == InputType.Alt){
					lastCheckpointCoordinatesAlt = new Vector3(this.transform.position.x, other.transform.position.y, this.transform.position.z);
				}
				
			}	
		}
		else if(this.tag == "GotoCheckpoint"){
			if(other.tag ==	"Player") {
				Debug.Log("Teleporting player to latest checkpoint");
				respawn(other);
			}
		}
    }
	
	
	public void resetCheckpoint(){
		lastCheckpointCoordinatesMain = Vector3.zero;
		lastCheckpointCoordinatesAlt = Vector3.zero;
	}
	
	public void respawn(Collider player){
		
		if(player.GetComponent<ControllerSchema>().SchemaKey == InputType.Main){
			lastCheckpointCoordinatesMain = new Vector3(this.transform.position.x, player.transform.position.y, this.transform.position.z);
		}
		if(player.GetComponent<ControllerSchema>().SchemaKey == InputType.Alt){
			lastCheckpointCoordinatesAlt = new Vector3(this.transform.position.x, player.transform.position.y, this.transform.position.z);
		}
		else
			Debug.Log("Error, no checkpoint set, or player could not be found");
	}
}