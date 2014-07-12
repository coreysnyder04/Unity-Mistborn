using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {
	
	public float walkSpeed = 8;
	public float runSpeed = 12;
	public float acceleration = 30;
	public float gravity = 20;
	public float jumpHeight = 12;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	
	private PlayerPhysics playerPhysics;
	private Animator animator;
	
	
	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(playerPhysics.movementStop){
			targetSpeed = 0;
			currentSpeed = 0;
		}

		if(playerPhysics.grounded){
			amountToMove.y = 0;
			if(Input.GetButtonDown("Jump")){
				amountToMove.y = jumpHeight;	
			}
		}

		animator.SetFloat("Speed", Mathf.Abs(currentSpeed));

		//Input 
		float speed = ( Input.GetButton("Run") ) ? runSpeed : walkSpeed;
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		// Set amount to move
		amountToMove.x = currentSpeed;
		amountToMove.y -= gravity * Time.deltaTime;
		playerPhysics.Move(amountToMove * Time.deltaTime);
		
	}
	
	// Increase n towards target by space
	private float IncrementTowards(float n, float target, float a){
		if(n == target){
			return n;
		}else{
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n)) ? n : target; // if n has now passed target then return target, otherwise reutrn n
		}
	}
}

