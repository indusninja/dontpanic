using UnityEngine;
using System.Collections;

public class PlayerEnergy : MonoBehaviour {
	
	//public Texture2D ChargeBarTexture;
	
	public GameObject heatingEffect;
	
	public float MaxCharge = 50;
	public float CurrentCharge = 50;
	public float MaxHeat = 50;
	public float CurrentHeat = 0;
	
	public float idleDrain = 0.1f;
	public float movingDrain = 1f;
	public float jumpDrain = 2f;
	
	public float MinTransferRate = 0f;
	public float MaxTransferRate = 25f;
	private float currentTransferRate = 0f;
	public float MaxEnergyTransferRange = 2f;
	
	private GameObject other;
	
	private AudioSource RadiationSound;
	
	// Use this for initialization
	void Start () {
		GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject target in potentialTargets) 
		{
			if(target != this.gameObject) {
				other = target;
				//return;
			}
		}
		
		if (heatingEffect){
			heatingEffect.active = false;
		}
		
		foreach(AudioSource source in GetComponents<AudioSource>())
		{
			if(source.clip.name.Contains("meltdown"))
				RadiationSound = source;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AcquireEnergy(bool active)
	{
		if(active)
			currentTransferRate = Mathf.Lerp(currentTransferRate, MaxTransferRate, 0.5f);
		else
			currentTransferRate = Mathf.Lerp(currentTransferRate, MinTransferRate, 0.5f);
		
		if(currentTransferRate == 0f)
		{
			//print("No transfer logic");
			return;
		}
		
		PlayerEnergy energyobj = other.GetComponent<PlayerEnergy>();
		if((energyobj.transform.position - transform.position).sqrMagnitude <= MaxEnergyTransferRange * MaxEnergyTransferRange)
		{
			CurrentCharge += energyobj.RequestEnergy(currentTransferRate, MaxCharge - CurrentCharge);
			//print("new charge: " + CurrentCharge);
		}
		else
		{
			//print("cannot transfer - not close enough");
		}
	}
	
	public float RequestEnergy(float percentage, float maxallowed)
	{
		float temp = Mathf.Min(maxallowed, CurrentCharge * percentage * 0.01f);
		CurrentCharge -= temp;
		//print("transferring: " + temp);
		return temp;
	}
	/*
	void OnGUI()
	{
	    if (Event.current.type == EventType.Repaint)
	    {
	        if(ChargeBarTexture!=null)
			{
				Rect increment = new Rect(10, 10, 5, 5);
				for(int i = 0; i < CurrentCharge; i++)
				{
					Rect drawRect = new Rect(increment.x * (i + 1), 
					                     increment.y + (increment.height * (MaxCharge - i)), 
					                     increment.width, 
					                     increment.height * (i + 1));
					//print(temp.ToString());
					Graphics.DrawTexture(drawRect, ChargeBarTexture);
				}
			}
	    }
	}*/
	
	public bool Use(int reqEnergy) {
		if (CurrentCharge >= reqEnergy) {
			//Enough energy
			CurrentCharge -= reqEnergy;
			if (CurrentCharge <= 0) {
				GetComponent<PlayerController>().die();
			}
			return true;
		} else {
			//not enough energy
			return false;
		}
	}
	
	//Always decharges player. Returns true if the energy count is still above zero
	public bool CanWalk()
	{
		if(CurrentCharge - movingDrain*Time.deltaTime > 0)
			return true;
		else return false;
	}
	
	public void DoWalk()
	{
		CurrentCharge -= movingDrain*Time.deltaTime;
	}
	
	//Charges the jump energy if enough energy remains. Returns true if enough energy left to jump
	public bool Jump()
	{
		if(CurrentCharge - jumpDrain > 0)
		{
			CurrentCharge -= jumpDrain;
			return true;
		}
		else 
			return false;
	}
	
	public bool IdleDrain()
	{
		CurrentCharge -= idleDrain*Time.deltaTime;
		return CurrentCharge > 0;
	}
	
	public void Recharge(int energy){
		CurrentCharge += energy;
		if (CurrentCharge > MaxCharge) {
			CurrentCharge = MaxCharge;
		}
	}
	
	public void Recharge(int energy, bool shouldOverheat){
		CurrentCharge += energy;
		if (CurrentCharge > MaxCharge) {
			CurrentHeat += (CurrentCharge - MaxCharge);
			CurrentCharge = MaxCharge;
			if(CurrentHeat>MaxHeat) {
				RadiationSound.Play();
				CurrentHeat = MaxHeat;
				GameController.LevelFail(gameObject);
			}
			if (heatingEffect){
				heatingEffect.active = true;
			}
			//print(CurrentCharge + " : " + CurrentHeat);
		}
	}
}
