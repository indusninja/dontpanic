using UnityEngine;
using System.Collections;

public enum SwitchState
{
	On,
	Off
}

public class Switchable : MonoBehaviour
{
	public SwitchableObject switchObject;
	public SwitchState switchState = SwitchState.Off;
	
	public bool OneTimeSwitch;
	public bool RequireBall;
	private bool AlreadySwitched;
	//public float OpenTime;
	
	//public bool SmoothMovement = true;
	
	//int timeelapsed = 0;
	//public int SwitchResetTime = 1000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	/*void FixedUpdate()
	{
		if(switchState == SwitchState.On)
		{
			timeelapsed++;
			print(timeelapsed);
			if(timeelapsed>SwitchResetTime)
			{
				switchState = SwitchState.Off;
				timeelapsed = 0;
				switchObject.Switch(this);
				print("Switch Off!");
			}
		}
	}*/
	
	void OnTriggerEnter(Collider other) {
		PlayerCarry ball = other.GetComponent<PlayerCarry>();
		if(RequireBall && !ball.IsCarrying())
			return;
		if (other.tag == "Player" && !AlreadySwitched) {
			if(OneTimeSwitch)
				AlreadySwitched = true;
			switchState = SwitchState.On;
			if(GetComponent<AudioSource>()!=null)
				if(!GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Play();
			switchObject.Switch(this);
			print("Switched on!");
		}
	}
	
	void OnTriggerExit(Collider other) {
	    PlayerCarry ball = other.GetComponent<PlayerCarry>();
		if(RequireBall && !ball.IsCarrying())
			return;
		if (other.tag == "Player" && (!OneTimeSwitch)) {
			switchState = SwitchState.Off;
			if(GetComponent<AudioSource>()!=null)
				if(GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Stop();
			switchObject.Switch(this);
			print("Switched off!");
		}
	}
}

