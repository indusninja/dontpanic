using UnityEngine;
using System.Collections;

public class PlayerCarry : MonoBehaviour {
	public GameObject carryingEffect;
	Carriable current;
	float maxTransferDistance = 5f;
	
	// Use this for initialization
	void Start () {
		if(carryingEffect) {
			carryingEffect.active = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void carry(Carriable item) {
		current = item;
		if (carryingEffect) {
			if (item){
				carryingEffect.active = true;
			} else {
				carryingEffect.active = false;
			}
		}
		//item.setCarrier(this);
	}
	
	public bool IsCarrying()
	{
		return (current!=null);
	}
	
	public void transfer() {
		if (!current){
			return;
		}
		//TODO: find nearby possible carriers
		GameObject[] potentialCarriers = GameObject.FindGameObjectsWithTag("Player");
		//print(potentialCarriers.Length + " found");
		PlayerCarry playerCarry = null;
		float shortestDistance = maxTransferDistance + 1f;
		foreach(GameObject potential in potentialCarriers) {
			//print(potential.name + " testing");
			if(potential != this.gameObject) {
				
				//Find the closest object within the maximum range!
				Vector3 distance = potential.transform.position - transform.position;
				if (distance.magnitude <= maxTransferDistance) {
					//print(potential.name + " withing range");
					if (playerCarry == null || distance.magnitude <= shortestDistance) {
						//print(potential.name + " picked");
						playerCarry = potential.GetComponent<PlayerCarry>();
						shortestDistance = distance.magnitude;
					}
				}
			}
			
		}
		//print(playerCarry);
		if (playerCarry) {
			//Transfer
			current.transferTo(playerCarry);
			current = null;
		}
	}
}
