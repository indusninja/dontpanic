using UnityEngine;
using System.Collections;

public class RechargePlayer : MonoBehaviour {
	
	public int lockPlayerFor = 1; //seconds
	public int rechargeRate = 1; //Energy/Sec

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnTriggerEnter(Collider other) {
		RechargeStation station;
		station = other.GetComponent<RechargeStation>();
		//Station was reached
		if (station) {
			//move the player to the recharging station
			Vector3 newPos = this.transform.position;
			newPos.x = station.transform.position.x;
			this.transform.position = newPos;
			PlayerController playerController = this.GetComponent<PlayerController>();
			lockPlayerControls(playerController);
			StartCoroutine(lockPlayerControls(playerController));
		}
	}
	
	IEnumerator lockPlayerControls(PlayerController control) {
		control.enabled = false;
		
		PlayerEnergy playerEnergy = control.GetComponent<PlayerEnergy>();
		int missing = playerEnergy.MaxCharge - playerEnergy.CurrentCharge;
		for(; playerEnergy.CurrentCharge < playerEnergy.MaxCharge; ) {
			playerEnergy.Recharge(rechargeRate);
			yield return new WaitForSeconds(1);
		}
		
		
		control.enabled = true;
	}*/
}

	
