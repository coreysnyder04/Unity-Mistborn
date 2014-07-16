using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : Entity {
	
	// Player Handling
	public float gravity = 20;
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float jumpHeight = 12;
	public float slideDeceleration = 10;
	private float initiateSlideThreshold;
	// Metals
	public float maxMetalDistance = 100;
	public float magneticForce = 50;


	
	// System
	private float animationSpeed;
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private float moveDirX;
	
	// States
	private bool jumping;
	private bool sliding;
	private bool stopSliding;
	private bool wallHolding;
	
	
	// Components
	private PlayerPhysics playerPhysics;
	private Animator animator;
	private GameManager manager;
	
	
	void Start () {
		// Init
		initiateSlideThreshold = walkSpeed +1;
		manager = Camera.main.GetComponent<GameManager>();

		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
		
		animator.SetLayerWeight(1,1);
	}
	
	void Update () {
		// Reset acceleration upon collision
		if (playerPhysics.movementStopped) {
			targetSpeed = 0;
			currentSpeed = 0;
		}


		// If player is touching the ground
		if (playerPhysics.grounded) {
			if(wallHolding){
				wallHolding = false;
				animator.SetBool("Wall Hold",false);	
			}
			amountToMove.y = 0;
			
			// Jump logic
			if (jumping) {
				jumping = false;
				animator.SetBool("Jumping",false);
			}
			
			// Slide logic
			if (sliding) {
				if (Mathf.Abs(currentSpeed) < .25f || stopSliding) {
					stopSliding = false;
					sliding = false;
					animator.SetBool("Sliding",false);
					playerPhysics.ResetCollider();
				}
			}

			// Slide Input
			if (Input.GetButtonDown("Slide") && Mathf.Abs(currentSpeed) > initiateSlideThreshold) {
				sliding = true;
				animator.SetBool("Sliding",true);
				targetSpeed = 0;
				
				playerPhysics.SetCollider(new Vector3(10.3f,1.5f,3), new Vector3(.35f,.75f,0));
			}
		}else{
			if(!wallHolding){
				if(playerPhysics.canWallHold){
					wallHolding = true;
					animator.SetBool("Wall Hold",true);
				}
			}

		}

		// Jump Input
		if (Input.GetButtonDown("Jump")) {

			if(sliding){
				stopSliding = true;
			}else if(playerPhysics.grounded || wallHolding){
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping",true);
				
				if(wallHolding){
					wallHolding = false;
					animator.SetBool("Wall Hold", false);
				}	
			}
		}
		
		// Set animator parameters
		animationSpeed = IncrementTowards(animationSpeed,Mathf.Abs(targetSpeed),acceleration);
		animator.SetFloat("Speed",animationSpeed);
		
		// Input
		moveDirX = Input.GetAxisRaw("Horizontal");
		if (!sliding) {
			float speed = (Input.GetButton("Run"))?runSpeed:walkSpeed;
			targetSpeed = moveDirX * speed;
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed,acceleration);
			
			// Face Direction
			if (moveDirX !=0 && !wallHolding) {
				transform.eulerAngles = (moveDirX>0)?Vector3.up * 180:Vector3.zero;
			}
		}
		else {
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed,slideDeceleration);
		}

		amountToMove = CheckMetalNearPlayer(amountToMove);

		// Set amount to move
		amountToMove.x = currentSpeed;
		if(wallHolding){
			amountToMove.x = 0;
			if(Input.GetAxisRaw("Vertical") != -1){
				amountToMove.y = 0;
			}
		}
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime, moveDirX);

	}

	Vector3 CheckMetalNearPlayer(Vector3 amountToMove){
		GameObject metal = GameObject.FindWithTag ("Metal");
		Rigidbody rb = metal.rigidbody;

		float distance = Vector3.Distance(transform.position, metal.transform.position);
		if( distance < maxMetalDistance){
			//magneticForce
			bool metalPush = Input.GetButton("Fire1");
			bool metalPull = Input.GetButton("Fire2");

			Vector3 force = (metal.transform.position - transform.position) * Mathf.Clamp(1f - ((metal.transform.position - transform.position).magnitude / 10), 0f, magneticForce);
			force.z = 0;

			if(metalPush){
				Debug.Log("PUSH:");
				Debug.Log ( force);
				rb.AddForce(force);
			}
			if(metalPull){
				Debug.Log ("PULL");
				Debug.Log ( force);
				rb.AddForce(-force);
			}
		}

		return amountToMove;

	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Checkpoint"){
			manager.SetCheckpoint(c.transform.position);
		}

		if(c.tag == "Finish"){
			manager.EndLevel();
		}
	}

	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
