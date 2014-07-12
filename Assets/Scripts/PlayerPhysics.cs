using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
	
	private BoxCollider collider;
	private Vector3 s; // Size
	private Vector3 c; // Center
	
	public LayerMask collisionMask;
	Ray ray;
	RaycastHit hit;
	private float skin = 0.005f; // Tiny bit of space between character and ground. Helps with bugs where ray casts through the ground and doesn't trigger a hit.
	
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool movementStop;
	
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider>();
		s = collider.size;
		c = collider.center;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	public void Move(Vector2 moveAmount){
		
		float deltaY = moveAmount.y;
		float deltaX = moveAmount.x;
		Vector2 p = transform.position; // Player Position
		
		float dir;
		float x;
		float y;
		grounded = false;
		
		
		// Top/Bottom Collisions
		for(int i = 0; i<3; i++){
			
			
			dir = Mathf.Sign(deltaY);
			x = (p.x + c.x - s.x/2) + s.x/2 * i; // Left, center and then right most point of collider;
			y = p.y + c.y + s.y/2 * dir; // Bottom of Collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
			Debug.DrawRay(ray.origin, ray.direction);
			
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask)){
				// Get distance between player and ground
				float dst = Vector3.Distance(ray.origin, hit.point);
				
				// Stop player's downward movement after coming within skin width of a collider
				if(dst > skin){
					deltaY = dst * dir - (skin * dir); // Last part is to add or subtract based on moving up/down
				}else{
					deltaY = 0;	
				}
				grounded = true;
				break;
			}
		}
		
		// Left & Right Collisions
		movementStop = false;
		for(int i = 0; i<3; i++){
			
			
			dir = Mathf.Sign(deltaX);
			x = p.x + c.x + s.x/2 * dir; // Left, center and then right most point of collider;
			y = p.y + c.y - s.y/2 + s.y/2 * i; // Bottom of Collider
			
			ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
			Debug.DrawRay(ray.origin, ray.direction);
			
			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask)){
				// Get distance between player and ground
				float dst = Vector3.Distance(ray.origin, hit.point);
				
				// Stop player's downward movement after coming within skin width of a collider
				if(dst > skin){
					deltaX = dst * dir - (skin * dir); // Last part is to add or subtract based on moving up/down
				}else{
					deltaX = 0;	
				}
				movementStop = true;
				break;
			}
		}
		
		Vector2 finalTransform = new Vector2(deltaX, deltaY);
		transform.Translate(finalTransform);
	}
}
