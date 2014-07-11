using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {
	
	private BoxCollider collider;
	private Vector3 s; // Size
	private Vector3 c; // Center
	
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
		Vector2 p = transform.position; // Player Position
		
		
		transform.Translate(moveAmount);
	}
}
