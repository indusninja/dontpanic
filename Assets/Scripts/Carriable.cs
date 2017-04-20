using UnityEngine;
using System.Collections;

public class Carriable : MonoBehaviour {
	
	public float animateSpeed = 5f;
	public float EffectTickRate = 1f; //Effects/Sec
	public int EnergyPerTick = 1;
	
	
	PlayerCarry carrier;
	bool animatingTo = false;
	float lastEffectTick = 0;
	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Renderer>().enabled) {
			Vector3 rotation = new Vector3(15,30,45) * Time.deltaTime;
			transform.Rotate(rotation);
		}
		
		if (carrier) { //When to tick The effect
			float passedTime = Time.timeSinceLevelLoad  - lastEffectTick;
			if (passedTime >= EffectTickRate) {
				tickEffect();
			}
		}
		
		if (animatingTo){
			Vector3 newPos = transform.position;
			Vector3 step = carrier.transform.position - newPos;
			//print(step);
			if (step.magnitude <= 0.01f ) {
				newPos = carrier.transform.position;
				animatingTo = false;
				this.GetComponent<Renderer>().enabled = false;
			} else {
				step.Normalize();
				step = step * animateSpeed;
				newPos += step * Time.deltaTime;
			}
			transform.position = newPos;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			setCarrier(other.GetComponent<PlayerCarry>());
			this.GetComponent<Collider>().enabled = false;
			//other.GetComponent<PlayerCarry>().carry(this);
		}
	}
	
	public void setCarrier(PlayerCarry player) {
		if (carrier != null && carrier != player) {
			//Cleanup stuff on the old carrier...
			carrier.carry(null);
			//position the pickup in the current carrier position
			transform.position = carrier.transform.position;
		}
		
		carrier = player;
		player.carry(this);
		animatingTo = true;
		this.GetComponent<Renderer>().enabled = true;
	}
	
	public void transferTo(PlayerCarry player) {
		setCarrier(player);
	}
	
	public void tickEffect() {
		//print("ticking");
		lastEffectTick = Time.timeSinceLevelLoad;
		//Apply Effect HERE!
		carrier.GetComponent<PlayerEnergy>().Recharge(EnergyPerTick, true);
		//TODO: also overheat
	}
}
