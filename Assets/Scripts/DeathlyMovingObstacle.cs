using UnityEngine;
using System.Collections;

public class DeathlyMovingObstacle : MonoBehaviour {
	public float MovementSpeed = 4f;
	public float EmergeSpeed = 2f;
	public float TimeToStayEmerged = 2f;
	public float IdleCooldown = 3f;
	public float TimeBeforeStart = 1.5f;
	
	private enum SpikeStatus {MovingBack, MovingForward, Idle, Emerged};
	private SpikeStatus currentStatus = SpikeStatus.Idle;
	private Vector3 startPos;
	
	//private bool 
	
	// Use this for initialization
	void Start () {
	
	}
	
	void Awake()
	{
		startPos = transform.position;
		currentStatus = DeathlyMovingObstacle.SpikeStatus.Idle;
		Invoke("FinishedIdle", TimeBeforeStart);
	}
	
	private void FinishedIdle(){
		currentStatus = SpikeStatus.MovingForward;
		Invoke("FullyEmerged", EmergeSpeed);
	}
	
	private void FullyEmerged ()
	{
		GetComponent<AudioSource>().Play();
		currentStatus = DeathlyMovingObstacle.SpikeStatus.Emerged;
		Invoke("TimeToWithdraw", TimeToStayEmerged);
	}
	
	private void TimeToWithdraw()
	{
		currentStatus = DeathlyMovingObstacle.SpikeStatus.MovingBack;
		Invoke("StartIdle", EmergeSpeed);
	}
	
	private void StartIdle()
	{
		transform.position = startPos;
		currentStatus = DeathlyMovingObstacle.SpikeStatus.Idle;
		Invoke("FinishedIdle",IdleCooldown);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(currentStatus == SpikeStatus.MovingForward)
		{
			transform.position += transform.forward * Time.fixedDeltaTime * MovementSpeed;
		}else if(currentStatus == DeathlyMovingObstacle.SpikeStatus.MovingBack)
		{
			transform.position += -transform.forward * Time.fixedDeltaTime * MovementSpeed;
		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {	
			print("[DeathlyObstacle] Player Touched and will die!!!");
			other.GetComponent<PlayerController>().die();
		}
	}
}
