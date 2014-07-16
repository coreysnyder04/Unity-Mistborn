using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GameObject currentPlayer;
	public GameObject player;
	private GameCamera cam;
	private Vector3 checkpoint;

	public static int levelCount = 3;
	public static int currentLevel = 1;
	
	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();

		if(GameObject.FindGameObjectWithTag("Spawn")){
			checkpoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;
		}

		SpawnPlayer(checkpoint);
		
	}

	public void SetCheckpoint(Vector3 newCheckpoint){
		checkpoint = newCheckpoint;
	}

	private void SpawnPlayer(Vector3 SpawnPos){
		
		currentPlayer = Instantiate(player, SpawnPos, Quaternion.identity) as GameObject;
        cam.SetTarget(currentPlayer.transform);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Respawn")){
			if(!currentPlayer){
				SpawnPlayer(checkpoint);	
			}
		}
	}

	public void EndLevel(){
		Debug.Log ("LEVEL" +currentLevel); 
		if(currentLevel < levelCount){
			currentLevel++;
			Application.LoadLevel("Level " + currentLevel);
		}else{
			Debug.Log ("END OF GAME");
		}
	}

}
