using UnityEngine;
using System.Collections;

public enum InputType
{
	Main,
	Alt,
	Undefined
}

[RequireComponent (typeof (PlayerController))]
public class ControllerSchema : MonoBehaviour 
{
	public InputType SchemaKey = InputType.Undefined;
	PlayerController controller;
	
	ArrayList stepSounds = new ArrayList();
	
	void Start()
	{
		controller = GetComponent<PlayerController>();
		
		foreach(AudioSource source in GetComponents<AudioSource>())
		{
			if(source.clip.name.Contains("step"))
			{
				stepSounds.Add(source);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch(SchemaKey)
		{
			case InputType.Main:
				if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
				{
					controller.AcceptInput("horizontal", Input.GetAxis("HorizontalP2"), false);
					PlayStepSound();
					//print(Input.GetAxis("Horizontal"));
					//ProcessAnimation("walk", Input.GetAxis("HorizontalP2"));
				}
				else
				{
					controller.AcceptInput("horizontal", 0f, false);
					//ProcessAnimation("idle", Input.GetAxis("HorizontalP2"));
				}
				if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
				{
					if(Input.GetAxis("VerticalP2")<-0.1f)
						controller.AcceptInput("crouch", Input.GetAxis("VerticalP2"), true);
					else if(Input.GetAxis("VerticalP2")>0.1f)
					{
						controller.AcceptInput("jump", 0f, false);
						PlayStepSound();
						//ProcessAnimation("jump", Input.GetAxis("HorizontalP2"));
					}
				}
				else
				{
					controller.AcceptInput("crouch", Input.GetAxis("Vertical"), false);
					//ProcessAnimation("idle", Input.GetAxis("HorizontalP2"));
				}
				
				if (Input.GetButton("Fire1P2")){
					print("[ControllerSchema] fire1P2");
					GetComponent<PlayerCarry>().transfer();
				}
				if(Input.GetButton("transferP2"))
				{
					GetComponent<PlayerEnergy>().AcquireEnergy(true);
				}
				else
				{
					GetComponent<PlayerEnergy>().AcquireEnergy(false);
				}
				break;
			case InputType.Alt:
				if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
				{
					controller.AcceptInput("horizontal", Input.GetAxis("Horizontal"), false);
					PlayStepSound();
					//print(Input.GetAxis("Horizontal"));
					//ProcessAnimation("walk", Input.GetAxis("Horizontal"));
				}
				else
				{
					controller.AcceptInput("horizontal", 0f, false);
					//ProcessAnimation("idle", Input.GetAxis("Horizontal"));
				}
				if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
				{
					if(Input.GetAxis("Vertical")<-0.1f)
						controller.AcceptInput("crouch", Input.GetAxis("Vertical"), true);
					else if(Input.GetAxis("Vertical")>0.1f)
					{
						controller.AcceptInput("jump", 0f, false);
						PlayStepSound();
						//ProcessAnimation("jump", Input.GetAxis("Horizontal"));
					}
				}
				else
				{
					controller.AcceptInput("crouch", Input.GetAxis("Vertical"), false);
					//ProcessAnimation("idle", Input.GetAxis("Horizontal"));
				}
				if (Input.GetButton("Fire1")){
					print("[ControllerSchema] fire1");
					GetComponent<PlayerCarry>().transfer();
				}
				if(Input.GetButton("transferP1"))
				{
					GetComponent<PlayerEnergy>().AcquireEnergy(true);
				}
				else
				{
					GetComponent<PlayerEnergy>().AcquireEnergy(false);
				}
				break;
			default:
				break;
		}
	}
	
	void PlayStepSound()
	{
		bool flag = false;
		foreach(AudioSource source in stepSounds)
		{
			flag |= source.isPlaying;
		}
		
		if(!flag)
		{
			AudioSource playAudio = (stepSounds[Random.Range(0,stepSounds.Count-1)] as AudioSource);
			//print("playing " + playAudio.clip.name + " sound on " + controller.name);
			playAudio.Play();
		}
	}
	
	void ProcessAnimation(string key, float direction)
	{
		//print("playing: " + key);
		/*switch(key)
		{
			case "walk":
				GetComponentInChildren<Animation>().CrossFade("walk");
				break;
			case "jump":
				GetComponentInChildren<Animation>().CrossFade("jump");
				break;
			case "idle":
				GetComponentInChildren<Animation>().CrossFade("idle");
				break;
			default:
				break;
		}*/
	}
}
