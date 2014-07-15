using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private GameObject currentPlayer;
	public GameObject player;
	private GameCamera cam;
	
	// Use this for initialization
	void Start () {
		cam = GetComponent<GameCamera>();
		SpawnPlayer(Vector3.zero);
		
	}

	private void SpawnPlayer(Vector3 SpawnPos){
		
		currentPlayer = Instantiate(player, SpawnPos, Quaternion.identity) as GameObject;
        cam.SetTarget(currentPlayer.transform);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Respawn")){
			if(!currentPlayer){
				SpawnPlayer(Vector3.zero);	
			}
		}
	}
}
