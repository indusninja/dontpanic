using UnityEngine;
using System.Collections;

public class RechargeStation : MonoBehaviour {
	
	//public float lockPlayerFor = 1; //seconds
	//public float rechargeRate = 10; //Energy/Sec
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			PlayerController playerController = other.GetComponent<PlayerController>();
			PlayerEnergy playerEnergy = playerController.GetComponent<PlayerEnergy>();
			playerEnergy.CurrentCharge = playerEnergy.MaxCharge;
			print("[RechargeStation] Player detected");
			/*RechargeStation station;
			station = other.GetComponent<RechargeStation>();*/
			//PlayerEnergy playerEnergy = other.GetComponent<PlayerEnergy>();
			//PlayerController playerController = other.GetComponent<PlayerController>();
			
			/*//Station was reached
			//if (station) {
				//move the player to the recharging station
				Vector3 newPos = playerController.transform.position;
				newPos.x = this.transform.position.x;
				playerController.transform.position = newPos;
				lockPlayerControls(playerController);
				StartCoroutine(lockPlayerControls(playerController));
			//}**/
		}
	}
	/*IEnumerator lockPlayerControls(PlayerController control) {
		control.enabled = false;
		
		PlayerEnergy playerEnergy = control.GetComponent<PlayerEnergy>();
		float missing = playerEnergy.MaxCharge - playerEnergy.CurrentCharge;
		for(; playerEnergy.CurrentCharge < playerEnergy.MaxCharge; ) {
			playerEnergy.Recharge(rechargeRate);
			if(playerEnergy.MaxCharge - playerEnergy.CurrentCharge < 1)
			{
				playerEnergy.CurrentCharge = playerEnergy.MaxCharge;
				break;
			}
			yield return new WaitForSeconds(1);
		}
		
		
		control.enabled = true;
	}*/
}
