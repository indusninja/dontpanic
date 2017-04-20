using UnityEngine;
using System.Collections;

public enum SwitchableObjectState
{
	Opening,
	Closing,
	Open,
	Closed
}

public enum SwitchingMode
{
	Target,
	Auto
}

public class SwitchableObject : MonoBehaviour {
	
	//Switchable ObjSwitch;
	//private int timeelapsed = 0;
	//public float OpenTime = 4;
	private float TimeStarted;
	//private float LastTimeOpened;
	public float SlidingSpeed = 10;
	public float StateSwitchTime = 4;
	public SwitchableObjectState ObjState = SwitchableObjectState.Closed;
	public SwitchingMode switchingMode = SwitchingMode.Auto;
	
	public bool SmoothMovement = true;
	
	
	Vector3 openPosition;
	Vector3 closedPosition;
	
	public Transform MoveTarget;
	
	// Use this for initialization
	void Start () {
		if(switchingMode == SwitchingMode.Auto)
		{
			switch(ObjState)
			{
				case SwitchableObjectState.Closed:
					closedPosition = transform.position;
					openPosition = transform.position + new Vector3(0, transform.localScale.y, 0);
					break;
				case SwitchableObjectState.Open:
					openPosition = transform.position;
					closedPosition = transform.position - new Vector3(0, transform.localScale.y, 0);
					break;
				default:
					break;
			}
		}
		else
		{
			openPosition = MoveTarget.position;
			closedPosition = transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 delta = Vector3.zero;
		//print(ObjState);
		switch(ObjState)
		{
			case SwitchableObjectState.Closing:
				//print("Slerping to closedposition : " + transform.position);
				if(SmoothMovement)
					transform.position = Vector3.Slerp(transform.position, closedPosition, (Time.timeSinceLevelLoad - TimeStarted)/StateSwitchTime);
				else
					transform.position = Vector3.Lerp(transform.position, closedPosition, (Time.timeSinceLevelLoad - TimeStarted)/StateSwitchTime);
				//transform.position = delta;
				//if(delta.sqrMagnitude > (closedPosition - openPosition).sqrMagnitude * 2f)
				if(Vector3.Distance(transform.position,closedPosition) < 0.01f)
				{
					print("State change!");
					ObjState = SwitchableObjectState.Closed;
				}
				break;
			case SwitchableObjectState.Opening:
				//print("Slerping to openposition : " + transform.position);
				if(SmoothMovement)
					transform.position = Vector3.Slerp(transform.position, openPosition, (Time.timeSinceLevelLoad - TimeStarted)/StateSwitchTime);
				else
					transform.position = Vector3.Lerp(transform.position, openPosition, (Time.timeSinceLevelLoad - TimeStarted)/StateSwitchTime);
				//transform.position = delta;
				//if(delta.sqrMagnitude>(closedPosition - openPosition).sqrMagnitude * 2)
				if(Vector3.Distance(transform.position,openPosition) < 0.01f)
				{
					print("State change!");
					//LastTimeOpened = Time.timeSinceLevelLoad;
					ObjState = SwitchableObjectState.Open;
				}
				break;
			/*case SwitchableObjectState.Open:
				if(Time.timeSinceLevelLoad > OpenTime + LastTimeOpened && SwitchingMode == SwitchState.Off)
				{
					//timeelapsed = 0;
					LastTimeOpened = Time.timeSinceLevelLoad;
					ObjState = SwitchableObjectState.Closing;
				}
				print("Door open: " + timeelapsed);
				break;*/
			default:
				break;
		}
	}
	
	public void Switch(Switchable obj)
	{
		//ObjSwitch = obj;
		switch(obj.switchState)
		{	
			case SwitchState.On:
				if(ObjState == SwitchableObjectState.Closing || ObjState == SwitchableObjectState.Closed)
				{
					ObjState = SwitchableObjectState.Opening;
					//timeelapsed = 0;
					TimeStarted = Time.timeSinceLevelLoad;
					//print("Opening Door!");
				}
				break;
			case SwitchState.Off:
				if(ObjState == SwitchableObjectState.Open || ObjState == SwitchableObjectState.Opening)
				{
					//if(timeelapsed > OpenTime)
					//{
						ObjState = SwitchableObjectState.Closing;
						TimeStarted = Time.timeSinceLevelLoad;
						//bReadyToClose = true;
						//print("Closing Door!");
					//}
				}
				break;
			default:
				break;
		}
	}
}
